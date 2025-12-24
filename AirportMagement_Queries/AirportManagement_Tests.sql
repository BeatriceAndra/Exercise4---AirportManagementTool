use AirportManagement

--Simple DB Checks--

--Check DB seed
SELECT COUNT(*) AS Users FROM AppUser;
SELECT COUNT(*) AS Airlines FROM Airline;
SELECT COUNT(*) AS Airports FROM Airport;
SELECT COUNT(*) AS Gates FROM Gate;
SELECT COUNT(*) AS Flights FROM Flight;
SELECT COUNT(*) AS FlightSchedules FROM FlightSchedule;
SELECT COUNT(*) AS Bookings FROM Booking;
SELECT COUNT(*) AS Tickets FROM Ticket;

--Check DB FK created corectly
SELECT 
    a.Name AS Airline,
    f.FlightNumber
FROM Airline a
JOIN Flight f ON f.AirlineId = a.Id
ORDER BY a.Name;

--Check if Origin is different from Destination
SELECT
    f.FlightNumber,
    o.IATACode AS Origin,
    d.IATACode AS Destination
FROM Flight f
JOIN Airport o ON f.OriginAirportId = o.Id
JOIN Airport d ON f.DestinationAirportId = d.Id;

--Check if Airport has at least 3 Gates
SELECT
    a.IATACode,
    COUNT(g.Id) AS GateCount
FROM Airport a
JOIN Gate g ON g.AirportId = a.Id
GROUP BY a.IATACode;

--See all flights
SELECT
    f.FlightNumber,
    fs.Id AS ScheduleId,
    fs.ScheduledDepartureUtc,
    fs.ScheduledArrivalUtc
FROM Flight f
JOIN FlightSchedule fs ON fs.FlightId = f.Id
ORDER BY f.FlightNumber, fs.ScheduledDepartureUtc;

--Gate usage
SELECT
    g.Code AS Gate,
    fs.ScheduledDepartureUtc,
    fs.ScheduledArrivalUtc
FROM FlightSchedule fs
JOIN Gate g ON fs.GateId = g.Id
ORDER BY g.Id, fs.ScheduledDepartureUtc;

--See seat capacity for each flight schedule
SELECT
    fs.Id AS ScheduleId,
    ac.Model,
    ac.SeatCapacity
FROM FlightSchedule fs
JOIN Aircraft ac ON fs.AssignedAircraftId = ac.Id;

--Full join
SELECT
    u.Name AS UserName,
    f.FlightNumber,
    fs.ScheduledDepartureUtc,
    fs.ScheduledArrivalUtc,
    g.Code AS Gate,
    t.FareClass,
    t.TotalPrice,
    b.ConfirmationCode
FROM Ticket t
JOIN Booking b ON t.BookingId = b.Id
JOIN AppUser u ON b.UserId = u.Id
JOIN FlightSchedule fs ON t.FlightScheduleId = fs.Id
JOIN Flight f ON fs.FlightId = f.Id
JOIN Gate g ON fs.GateId = g.Id
ORDER BY fs.ScheduledDepartureUtc;

SELECT * FROM sys.tables WHERE name LIKE 'AspNet%'

SELECT name FROM sys.tables WHERE name LIKE 'AspNet%'

