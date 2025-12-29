use AirportManagement

--CONSTRAINTS--

--Aircraft--

ALTER TABLE Aircraft
ADD CONSTRAINT CK_Aircraft_SeatCapacity
CHECK (SeatCapacity > 0);

--Gate--

ALTER TABLE Gate
ADD CONSTRAINT UQ_Gate_Airport_Code
UNIQUE (AirportId, Code)

--Flight--

ALTER TABLE Flight
ADD CONSTRAINT CK_Flight_Origin_Destination
CHECK (OriginAirportId <> DestinationAirportId);

ALTER TABLE Flight
ADD CONSTRAINT CK_Flight_FlightNumber_Format
CHECK (FlightNumber LIKE '[A-Z][A-Z]%' AND FlightNumber LIKE '%[0-9]');

--Flight schedule--

ALTER TABLE FlightSchedule
ADD CONSTRAINT CK_FlightSchedule_Time
CHECK (ScheduledArrivalUtc > ScheduledDepartureUtc)

--Booking--
ALTER TABLE Booking
ADD CONSTRAINT CK_Booking_Quantity
CHECK (Quantity > 0)

ALTER TABLE Booking
ADD CONSTRAINT CK_Booking_ConfirmationCode
CHECK (LEN(ConfirmationCode) BETWEEN 6 AND 8 AND ConfirmationCode NOT LIKE '%[^A-Z0-9]%');

--Ticket--
ALTER TABLE Ticket
ADD CONSTRAINT CK_Ticket_FareClass
CHECK (FareClass IN ('Y','M','J','F'))

ALTER TABLE Ticket
ADD CONSTRAINT CK_Ticket_Prices
CHECK (BasePrice >= 0 AND Taxes >= 0)