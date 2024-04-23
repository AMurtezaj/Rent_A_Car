using Data.DTOs.BookingDtos;
using Data.DTOs.UserDtos;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Mailing
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendForgotPasswordEmail(User user, string token);
        Task SendVerifyAccountEmail(UserCreateDto user);
        Task SendEmailToCostumerWhenBookingAcceptedAsync(User user, BookingDto booking);

    }
}
