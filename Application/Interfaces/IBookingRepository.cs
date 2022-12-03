using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookingRepository
    {
        Booking CreateBooking(Booking booking);
        Booking DeleteBooking(int id);
        Booking ReadBooking(int id);
        Coach ChangeAvailableTimes(AvailableTimesDTO dto);
    }
}
