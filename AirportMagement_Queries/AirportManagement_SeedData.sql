use AirportManagement

--Seed database--

--AppUsers--

INSERT INTO AppUser (Id, Name, Email) VALUES
(1, 'Alice Popescu', 'alice.popescu@email.com'),
(2, 'Bogdan Ionescu', 'bogdan.ionescu@email.com'),
(3, 'Clara Marinescu', 'clara.marinescu@email.com');

--BookingStatus--

INSERT INTO BookingStatus (Id, Status) VALUES
(1, 'Active'),
(2, 'Cancelled');

--FlightStatus--

INSERT INTO FlightStatus (Id, Status) VALUES
(1, 'Planned'),
(2, 'Boarding'),
(3, 'Departed'),
(4, 'Cancelled'),
(5, 'Delayed');

--Airlines--

INSERT INTO Airline (Id, IATACode, Name) VALUES
(1, 'BA', 'British Airways'),
(2, 'AF', 'Air France'),
(3, 'LH', 'Lufthansa'),
(4, 'RY', 'Ryanair');

--Addresses--

INSERT INTO Address (Id, Country, City, Street) VALUES
(1, 'UK', 'London', 'Heathrow Airport Rd'),
(2, 'France', 'Paris', 'Charles de Gaulle Rd'),
(3, 'Germany', 'Frankfurt', 'Frankfurt Airport Rd'),
(4, 'Spain', 'Madrid', 'Adolfo Suárez Madrid-Barajas Rd'),
(5, 'Italy', 'Rome', 'Fiumicino Airport Rd'),
(6, 'Netherlands', 'Amsterdam', 'Schiphol Rd');

--Airports--

INSERT INTO Airport (Id, IATACode, Name, TimeZone, AddressId) VALUES
(1, 'LHR', 'Heathrow', 'GMT', 1),
(2, 'CDG', 'Charles de Gaulle', 'CET', 2),
(3, 'FRA', 'Frankfurt Airport', 'CET', 3),
(4, 'MAD', 'Madrid-Barajas', 'CET', 4),
(5, 'FCO', 'Fiumicino', 'CET', 5),
(6, 'AMS', 'Schiphol', 'CET', 6);

--Gates--

INSERT INTO Gate (Id, AirportId, Code) VALUES
-- LHR
(1, 1, 'A1'),
(2, 1, 'A2'),
(3, 1, 'A3'),
(4, 1, 'A4'),
(5, 1, 'A5'),
-- CDG
(6, 2, 'B1'),
(7, 2, 'B2'),
(8, 2, 'B3'),
(9, 2, 'B4'),
(10, 2, 'B5'),
-- FRA
(11, 3, 'C1'),
(12, 3, 'C2'),
(13, 3, 'C3'),
-- MAD
(14, 4, 'D1'),
(15, 4, 'D2'),
(16, 4, 'D3'),
-- FCO
(17, 5, 'E1'),
(18, 5, 'E2'),
(19, 5, 'E3'),
-- AMS
(20, 6, 'F1'),
(21, 6, 'F2'),
(22, 6, 'F3');

--Aircrafts--

INSERT INTO Aircraft (Id, TailNumber, Model, SeatCapacity) VALUES
(1, 'G-BA001', 'Boeing 777', 300),
(2, 'G-BA002', 'Airbus A320', 180),
(3, 'F-AF001', 'Airbus A350', 320),
(4, 'D-LH001', 'Boeing 747', 400),
(5, 'EI-RY001', 'Boeing 737', 200),
(6, 'EI-RY002', 'Boeing 737', 200);

--Flights--

INSERT INTO Flight (Id, AirlineId, FlightNumber, OriginAirportId, DestinationAirportId, DefaultAircraftId, IsActive) VALUES
(1, 1, 'BA123', 1, 2, 1, 1),
(2, 1, 'BA456', 1, 3, 2, 1),
(3, 2, 'AF101', 2, 6, 3, 1),
(4, 3, 'LH202', 3, 4, 4, 1),
(5, 4, 'RY303', 4, 5, 5, 1),
(6, 4, 'RY404', 6, 1, 6, 1);

--FlightsSchedules--

INSERT INTO FlightSchedule (Id, FlightId, ScheduledDepartureUtc, ScheduledArrivalUtc, GateId, AssignedAircraftId, FlightStatusId) VALUES
(1, 1, '2025-12-15 07:00', '2025-12-15 09:00', 1,  1, 1),
(2, 1, '2025-12-15 22:00', '2025-12-16 00:00', 2,  1, 1),
(3, 2, '2025-12-15 09:00', '2025-12-15 11:00', 3,  2, 1),
(4, 2, '2025-12-15 21:00', '2025-12-15 23:00', 4,  2, 1),
(5, 3, '2025-12-15 08:00', '2025-12-15 10:00', 6,  3, 1),
(6, 3, '2025-12-15 20:00', '2025-12-15 22:00', 7,  3, 1),
(7, 4, '2025-12-15 06:00', '2025-12-15 08:30', 11, 4, 1),
(8, 4, '2025-12-15 18:00', '2025-12-15 20:30', 12, 4, 1),
(9, 5, '2025-12-15 07:30', '2025-12-15 09:30', 14, 5, 1),
(10, 5, '2025-12-15 19:30', '2025-12-15 21:30', 15, 6, 1),
(11, 6, '2025-12-15 08:30', '2025-12-15 10:30', 20, 6, 1),
(12, 6, '2025-12-15 20:30', '2025-12-15 22:30', 21, 6, 1);

--Bookings--

INSERT INTO Booking (Id, UserId, BookingStatusId, ConfirmationCode, Quantity, CreatedUtc) VALUES
(1, 1, 1, 'ABC123', 2, '2025-12-15 10:00'),
(2, 1, 2, 'DEF456', 2, '2025-12-15 11:00'),
(3, 2, 1, 'GHI789', 1, '2025-12-15 09:00'),
(4, 2, 2, 'JKL012', 2, '2025-12-15 12:00'),
(5, 3, 1, 'MNO345', 2, '2025-12-15 08:00');

--Tickets--

INSERT INTO Ticket (Id, BookingId, FlightScheduleId, FareClass, BasePrice, Taxes, Currency, IsRefundable, SeatNumber, PassengerFullName, PassengerEmail) VALUES
(1, 1, 1, 'Y', 100, 20, 'EUR', 1, '1A', 'Alice Popescu', 'alice.popescu@email.com'),
(2, 1, 1, 'M', 150, 30, 'EUR', 1, '1B', 'Alice Popescu', 'alice.popescu@email.com'),
(3, 2, 2, 'Y', 100, 20, 'EUR', 1, '2A', 'Alice Popescu', 'alice.popescu@email.com'),
(4, 2, 2, 'M', 150, 30, 'EUR', 1, '2B', 'Alice Popescu', 'alice.popescu@email.com'),
(5, 3, 3, 'Y', 120, 25, 'EUR', 1, '3A', 'Bogdan Ionescu', 'bogdan.ionescu@email.com'),
(6, 4, 4, 'M', 170, 35, 'EUR', 1, '3B', 'Bogdan Ionescu', 'bogdan.ionescu@email.com'),
(7, 4, 4, 'Y', 120, 25, 'EUR', 1, '4A', 'Bogdan Ionescu', 'bogdan.ionescu@email.com'),
(8, 4, 4, 'M', 170, 35, 'EUR', 1, '4B', 'Bogdan Ionescu', 'bogdan.ionescu@email.com'),
(9, 5, 5, 'Y', 90, 15, 'EUR', 1, '5A', 'Clara Marinescu', 'clara.marinescu@email.com'),
(10, 5, 5, 'M', 140, 25, 'EUR', 1, '5B', 'Clara Marinescu', 'clara.marinescu@email.com');