use AirportManagement

--INDEXES--

--Flight--

CREATE NONCLUSTERED INDEX IX_Flight_Airline_FlightNumber
ON Flight(AirlineId, FlightNumber);

CREATE NONCLUSTERED INDEX IX_Flight_Origin_Destination
ON Flight(OriginAirportId, DestinationAirportId);

--Flight Schedule

CREATE NONCLUSTERED INDEX IX_FlightSchedule_Flight_Departure
ON FlightSchedule(FlightId, ScheduledDepartureUtc);

--Booking--

CREATE UNIQUE INDEX IX_Booking_ConfirmationCode
ON Booking(ConfirmationCode);

--Ticket--

CREATE NONCLUSTERED INDEX IX_Ticket_FlightSchedule_FareClass
ON Ticket(FlightScheduleId, FareClass);