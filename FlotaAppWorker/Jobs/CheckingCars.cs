using FlotaAppWorker.Data;
using FlotaAppWorker.Helpers;
using FlotaAppWorker.Interfaces;
using FlotaAppWorker.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FlotaAppWorker.Jobs
{
    [DisallowConcurrentExecution]
    public class CheckingCars : IJob
    {
       
        private readonly ICarRepository _carRepository;
        private readonly IConfiguration _config;
        private readonly HttpClient _client;
        private readonly IEmailSender _emailSender;
        private readonly IErrorEmailSender _errorEmailSender;

        public CheckingCars(ICarRepository carRepository, IConfiguration config, HttpClient client, IEmailSender emailSender, IErrorEmailSender errorEmailSender)
        {
            _carRepository = carRepository;
            _config = config;
            _client = client;
            _emailSender = emailSender;
            _errorEmailSender = errorEmailSender;
        }

        public Task Execute(IJobExecutionContext context)
        {
            string AnnaPhoneNumber = _config["AnnaPhoneNumber"];

            var cars = _carRepository.GetCarsAsync().Result;

            foreach (var car in cars)
            {

                var userPhoneNumber = car.User == null ? AnnaPhoneNumber : car.User.PhoneNumber;

                if ((car.Service.ServiceExpirationDate - DateTime.Now.Date).Days < 14)
                {
                    var difference = (car.Service.ServiceExpirationDate - DateTime.Now.Date).Days < 0 ? 0 : (car.Service.ServiceExpirationDate - DateTime.Now.Date).Days;
                    string message = GenerateMessage("Service Date", car, difference);
                    _emailSender.Send(car, message);

                    string smsMessage = message.Replace("<b>", "").Replace("<br>", "").Replace("</b>", "");
                    sendSms(userPhoneNumber, smsMessage);
                }

                if ((car.TechnicalExamination.TechnicalExaminationExpirationDate - DateTime.Now.Date).Days < 14)
                {
                    var difference = (car.TechnicalExamination.TechnicalExaminationExpirationDate - DateTime.Now.Date).Days < 0 ? 0 : (car.TechnicalExamination.TechnicalExaminationExpirationDate - DateTime.Now.Date).Days;
                    string message = GenerateMessage("Technical Examination", car, difference);
                    _emailSender.Send(car, message);

                    string smsMessage = message.Replace("<b>", "").Replace("<br>", "").Replace("</b>", "");
                    sendSms(userPhoneNumber, smsMessage);
                }

                if ((car.CarInsurance.ExpirationDate - DateTime.Now.Date).Days < 14)
                {
                   
                    var difference = (car.CarInsurance.ExpirationDate - DateTime.Now.Date).Days < 0 ? 0 : (car.CarInsurance.ExpirationDate - DateTime.Now.Date).Days;                 
                    string message = GenerateMessage("Car Insurance", car, difference);                   
                    _emailSender.Send(car, message);

                    string smsMessage = message.Replace("<b>", "").Replace("<br>", "").Replace("</b>", "");
                    sendSms(userPhoneNumber, smsMessage);
                }
                if(car.Service.NextServiceMeterStatus - car.MeterStatus <= 1500)
                {
                    var difference = car.Service.NextServiceMeterStatus - car.MeterStatus < 0 ? 0 : car.Service.NextServiceMeterStatus - car.MeterStatus;
                    string message = GenerateMessage("Service Meter Status", car, difference);
                    _emailSender.Send(car, message);

                    string smsMessage = message.Replace("<b>", "").Replace("<br>", "").Replace("</b>", "");
                    sendSms(userPhoneNumber, smsMessage);
                }
            }

            return Task.CompletedTask;

        }

        private string GenerateMessage(string field, Car car, int difference)
        {
            string message = field switch
            {
                "Car Insurance" => $"Dzień Dobry<br><br> Przypominamy, że dla pojazdu: " +
                                            $"{car.Brand} {car.Model} - {car.RegistrationNumber} za <b>{difference} dni</b> kończy się ubezpieczenie." +
                                            $"<br><br> Pozdrawiam.",

                "Service Date" => $"Dzień Dobry <br><br> Przypominamy, że dla pojazdu: " +
                                             $"{car.Brand} {car.Model} - {car.RegistrationNumber} za <b>{difference} dni</b> mija termin wizyty w serwisie." +
                                            $"<br><br> Pozdrawiam.",

                "Technical Examination" => $"Dzień Dobry <br><br> Przypominamy, że dla pojazdu: " +
                                            $"{car.Brand} {car.Model} - {car.RegistrationNumber} za <b>{difference} dni</b> mija termin badania technicznego." +
                                            $"<br><br> Pozdrawiam.",

                "Service Meter Status" => $"Dzień Dobry<br><br> Przypominamy, że dla pojazdu: " +
                                            $"{car.Brand} {car.Model} - {car.RegistrationNumber} za <b>{difference} km</b> wymagana jest wizyta w serwisie." +
                                            $"<br><br> Pozdrawiam.",

                _ => "Brak pozycji w switch case"

            };

            return message;
        }

        private void sendSms(string phoneNumber, string message)
        {
            try
            {
                string smsApi = _config["SmsApi"];
                var sms = new SMS(phoneNumber, message);

                var result = _client.PostAsJsonAsync(smsApi, sms).Result;

                if (!result.IsSuccessStatusCode)
                {
                    var emailMessage = $"Dzień Dobry <br><br> Nie udało się wysłać SMS na numer: <b>{phoneNumber}</b> " +
                                         $"o treści : {message} <br><br> Kod błędu to: <b>{result.StatusCode}</b>" +
                                         $"<br><br> Pozdrawiam.";
                    _errorEmailSender.Send(result.StatusCode.ToString(), emailMessage);
                }
            }
            catch (Exception ex)
            {
                var emailMessage = $"Dzień Dobry <br><br> Nie udało się wysłać SMS na numer: <b>{phoneNumber}</b> " +
                                         $"o treści : {message} <br><br> z powodu błędu: <b>{ex.Message}</b>" +
                                         $"<br><br> Pozdrawiam.";

                _errorEmailSender.Send(null, emailMessage);

            }
        }

        

    }
}
