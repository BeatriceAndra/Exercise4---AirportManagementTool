create database AirportManagement;

use AirportManagement

--Simple tables without FK--

CREATE TABLE Airline (
    Id INT PRIMARY KEY,
    IATACode NCHAR(2) NOT NULL UNIQUE,
    Name NVARCHAR(100) NOT NULL
)

CREATE TABLE Address (
    Id INT PRIMARY KEY,
    Country NVARCHAR(100) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    Street NVARCHAR(200) NOT NULL
);

CREATE TABLE BookingStatus (
    Id INT PRIMARY KEY,
    Status NVARCHAR(50) NOT NULL
);

CREATE TABLE FlightStatus (
    Id INT PRIMARY KEY,
    Status NVARCHAR(50) NOT NULL
);

CREATE TABLE AppUser (
    Id INT PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    Email NVARCHAR(120) NOT NULL
);

CREATE TABLE Aircraft (
    Id INT PRIMARY KEY,
    TailNumber NVARCHAR(10) NOT NULL UNIQUE,
    Model NVARCHAR(60) NOT NULL,
    SeatCapacity INT NOT NULL
);

--Tables with FK--

CREATE TABLE Airport (
    Id INT PRIMARY KEY,
    IATACode NCHAR(3) NOT NULL UNIQUE,
    Name NVARCHAR(120) NOT NULL,
    TimeZone NVARCHAR(64) NOT NULL,
    AddressId INT NOT NULL,

    CONSTRAINT FK_Airport_Address
        FOREIGN KEY (AddressId) REFERENCES Address(Id)
);

CREATE TABLE Gate (
    Id INT PRIMARY KEY,
    AirportId INT NOT NULL,
    Code NVARCHAR(10) NOT NULL,

    CONSTRAINT FK_Gate_Airport
        FOREIGN KEY (AirportId) REFERENCES Airport(Id)
);

CREATE TABLE Flight (
    Id INT PRIMARY KEY,
    AirlineId INT NOT NULL,
    FlightNumber NVARCHAR(8) NOT NULL,
    OriginAirportId INT NOT NULL,
    DestinationAirportId INT NOT NULL,
    DefaultAircraftId INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Flight_Airline
        FOREIGN KEY (AirlineId) REFERENCES Airline(Id),

    CONSTRAINT FK_Flight_OriginAirport
        FOREIGN KEY (OriginAirportId) REFERENCES Airport(Id),

    CONSTRAINT FK_Flight_DestinationAirport
        FOREIGN KEY (DestinationAirportId) REFERENCES Airport(Id),

    CONSTRAINT FK_Flight_DefaultAircraft
        FOREIGN KEY (DefaultAircraftId) REFERENCES Aircraft(Id),
);

CREATE TABLE FlightSchedule (
    Id INT PRIMARY KEY,
    FlightId INT NOT NULL,
    ScheduledDepartureUtc DATETIME2 NOT NULL,
    ScheduledArrivalUtc DATETIME2 NOT NULL,
    GateId INT NULL,
    AssignedAircraftId INT NULL,
    FlightStatusId INT NOT NULL,

    CONSTRAINT FK_FlightSchedule_Flight
        FOREIGN KEY (FlightId) REFERENCES Flight(Id),

    CONSTRAINT FK_FlightSchedule_Gate
        FOREIGN KEY (GateId) REFERENCES Gate(Id),

    CONSTRAINT FK_FlightSchedule_AssignedAircraft
        FOREIGN KEY (AssignedAircraftId) REFERENCES Aircraft(Id),

    CONSTRAINT FK_FlightSchedule_FlightStatus
        FOREIGN KEY (FlightStatusId) REFERENCES FlightStatus(Id),
);

CREATE TABLE Booking (
    Id INT PRIMARY KEY,
    UserId INT NOT NULL,
    BookingStatusId INT NOT NULL,
    ConfirmationCode NVARCHAR(8) NOT NULL UNIQUE,
    Quantity INT NOT NULL,
    CreatedUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Booking_User
        FOREIGN KEY (UserId) REFERENCES AppUser(Id),

    CONSTRAINT FK_Booking_Status
        FOREIGN KEY (BookingStatusId) REFERENCES BookingStatus(Id),
);

CREATE TABLE Ticket (
    Id INT PRIMARY KEY,
    BookingId INT NOT NULL,
    FlightScheduleId INT NOT NULL,
    FareClass NVARCHAR(2) NOT NULL,
    BasePrice DECIMAL(10,2) NOT NULL,
    Taxes DECIMAL(10,2) NOT NULL,
    TotalPrice AS (BasePrice + Taxes) PERSISTED,
    Currency NCHAR(3) NOT NULL,
    IsRefundable BIT NOT NULL DEFAULT 0,
    SeatNumber NVARCHAR(4) NULL,
    PassengerFullName NVARCHAR(120) NOT NULL,
    PassengerEmail NVARCHAR(120) NOT NULL,

    CONSTRAINT FK_Ticket_Booking
        FOREIGN KEY (BookingId) REFERENCES Booking(Id),

    CONSTRAINT FK_Ticket_FlightSchedule
        FOREIGN KEY (FlightScheduleId) REFERENCES FlightSchedule(Id),
);