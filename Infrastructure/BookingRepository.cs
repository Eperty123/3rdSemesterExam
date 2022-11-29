using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class BookingRepository : IBookingRepository
    {
        private DatabaseContext _context;

        public BookingRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Booking CreateBooking(Booking booking)
        {
            _context.BookingTable.Add(booking);
            _context.SaveChanges();
            return booking;
        }

        public Booking DeleteBooking(int id)
        {
            var bookingToDelete = _context.BookingTable.Find(id) ?? throw new KeyNotFoundException();
            _context.BookingTable.Remove(bookingToDelete);
            _context.SaveChanges();
            return bookingToDelete;
        }
    }
}
