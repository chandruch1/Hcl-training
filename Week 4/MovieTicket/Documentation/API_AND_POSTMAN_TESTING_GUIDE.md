# API & Postman Testing Guide — Movie Ticket Booking System

This guide provides step-by-step instructions for testing every endpoint of the **Movie Ticket Booking System** in Postman or any HTTP client.

---

## 🚀 1. Setup & Postman Prerequisites

### 1.1 Start the API Server
Open PowerShell in the project directory and run:
```powershell
cd "c:\Users\CHANDRU\OneDrive\Desktop\hcl\Week 4\MovieTicket\MovieTicketBooking"
dotnet run
```
Check the output URL. By default, it will listen on `http://localhost:5173` (or `https://localhost:7173` / `http://localhost:5000`).

### 1.2 Postman Environment Variable
Create a Postman environment or set a variable:
* **Variable:** `baseUrl`
* **Initial Value:** `http://localhost:5173` (adjust if your local port differs)

### 1.3 Recommended Execution Flow
Because relational database foreign keys must be respected, test the endpoints in the following order:
1. **Theatres** (creates `theatreId`)
2. **Screens** (references `theatreId`, creates `screenId`)
3. **Seats** (references `screenId`, creates `seatId`s)
4. **Movies** (creates `movieId`)
5. **Shows** (references `movieId` and `screenId`, auto-generates `ShowSeats`)
6. **Bookings** (references `showId` and `seatIds`, locks seats to `Booked`)
7. **Cancellation** (cancels booking, reverts seats to `Available`)

---

## 🏢 2. Theatres API

### 2.1 Create Theatre
* **Method:** `POST`
* **URL:** `{{baseUrl}}/api/theatres`
* **Header:** `Content-Type: application/json`
* **Body:**
```json
{
  "name": "PVR Cinemas IMAX",
  "address": "Phoenix Marketcity, Velachery",
  "city": "Chennai",
  "state": "Tamil Nadu",
  "phone": "+91-9876543210",
  "email": "chennai@pvrcinemas.com"
}
```
* **Expected Status:** `201 Created`
* **Sample Response:**
```json
{
  "theatreId": 1,
  "name": "PVR Cinemas IMAX",
  "address": "Phoenix Marketcity, Velachery",
  "city": "Chennai",
  "state": "Tamil Nadu",
  "phone": "+91-9876543210",
  "email": "chennai@pvrcinemas.com",
  "isActive": true,
  "createdAt": "2026-11-02T04:30:00Z"
}
```

---

### 2.2 Get All Theatres
* **Method:** `GET`
* **URL:** `{{baseUrl}}/api/theatres`
* **Expected Status:** `200 OK`
* **Sample Response:**
```json
[
  {
    "theatreId": 1,
    "name": "PVR Cinemas IMAX",
    "address": "Phoenix Marketcity, Velachery",
    "city": "Chennai",
    "state": "Tamil Nadu",
    "phone": "+91-9876543210",
    "email": "chennai@pvrcinemas.com",
    "isActive": true,
    "createdAt": "2026-11-02T04:30:00Z"
  }
]
```

---

### 2.3 Get Theatre By ID
* **Method:** `GET`
* **URL:** `{{baseUrl}}/api/theatres/1`
* **Expected Status:** `200 OK`

---

### 2.4 Update Theatre
* **Method:** `PUT`
* **URL:** `{{baseUrl}}/api/theatres/1`
* **Header:** `Content-Type: application/json`
* **Body:**
```json
{
  "name": "PVR INOX Grand",
  "address": "Phoenix Marketcity, Velachery",
  "city": "Chennai",
  "state": "Tamil Nadu",
  "phone": "+91-9876543210",
  "email": "grand@pvrcinemas.com",
  "isActive": true
}
```
* **Expected Status:** `200 OK`

---

## 🖥️ 3. Screens API

### 3.1 Create Screen
* **Method:** `POST`
* **URL:** `{{baseUrl}}/api/screens`
* **Header:** `Content-Type: application/json`
* **Body:**
```json
{
  "theatreId": 1,
  "screenName": "Screen 1 - IMAX Laser",
  "totalSeats": 60,
  "screenType": "IMAX"
}
```
* **Expected Status:** `201 Created`
* **Sample Response:**
```json
{
  "screenId": 1,
  "theatreId": 1,
  "theatreName": "PVR INOX Grand",
  "screenName": "Screen 1 - IMAX Laser",
  "totalSeats": 60,
  "screenType": "IMAX",
  "isActive": true,
  "createdAt": "2026-11-02T04:31:00Z"
}
```

---

### 3.2 Get Screens by Theatre ID
* **Method:** `GET`
* **URL:** `{{baseUrl}}/api/screens/theatre/1`
* **Expected Status:** `200 OK`

---

### 3.3 Get Screen By ID
* **Method:** `GET`
* **URL:** `{{baseUrl}}/api/screens/1`
* **Expected Status:** `200 OK`

---

## 💺 4. Seats API

### 4.1 Bulk Create Seats (Recommended)
* **Method:** `POST`
* **URL:** `{{baseUrl}}/api/seats/bulk`
* **Header:** `Content-Type: application/json`
* **Body:**
```json
{
  "screenId": 1,
  "seats": [
    { "rowLetter": "A", "columnNumber": 1, "seatType": "Regular", "price": 180.00 },
    { "rowLetter": "A", "columnNumber": 2, "seatType": "Regular", "price": 180.00 },
    { "rowLetter": "A", "columnNumber": 3, "seatType": "Regular", "price": 180.00 },
    { "rowLetter": "B", "columnNumber": 1, "seatType": "VIP", "price": 250.00 },
    { "rowLetter": "B", "columnNumber": 2, "seatType": "VIP", "price": 250.00 }
  ]
}
```
* **Expected Status:** `200 OK`
* **Sample Response:**
```json
[
  {
    "seatId": 1,
    "screenId": 1,
    "screenName": "Screen 1 - IMAX Laser",
    "rowLetter": "A",
    "columnNumber": 1,
    "seatNumber": "A1",
    "seatType": "Regular",
    "price": 180.00,
    "isActive": true,
    "createdAt": "2026-11-02T04:32:00Z"
  },
  {
    "seatId": 2,
    "screenId": 1,
    "screenName": "Screen 1 - IMAX Laser",
    "rowLetter": "A",
    "columnNumber": 2,
    "seatNumber": "A2",
    "seatType": "Regular",
    "price": 180.00,
    "isActive": true,
    "createdAt": "2026-11-02T04:32:00Z"
  }
]
```

---

### 4.2 Create Single Seat
* **Method:** `POST`
* **URL:** `{{baseUrl}}/api/seats`
* **Header:** `Content-Type: application/json`
* **Body:**
```json
{
  "screenId": 1,
  "rowLetter": "C",
  "columnNumber": 1,
  "seatType": "Premium",
  "price": 300.00
}
```
* **Expected Status:** `201 Created`

---

### 4.3 Get Seats by Screen ID
* **Method:** `GET`
* **URL:** `{{baseUrl}}/api/seats/screen/1`
* **Expected Status:** `200 OK`

---

### 4.4 Duplicate Seat Coordinate Test (Business Rule Verification)
* Try posting seat `A1` again for Screen 1.
* **Expected Status:** `400 Bad Request`
* **Sample Response:**
```json
{
  "statusCode": 400,
  "message": "Seat A1 already exists in screen 1.",
  "errorType": "InvalidOperationException",
  "timestamp": "2026-11-02T04:33:00Z"
}
```

---

## 🎬 5. Movies API

### 5.1 Create Movie
* **Method:** `POST`
* **URL:** `{{baseUrl}}/api/movies`
* **Header:** `Content-Type: application/json`
* **Body:**
```json
{
  "title": "Interstellar",
  "description": "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
  "genre": "Sci-Fi / Adventure",
  "language": "English",
  "durationMinutes": 169,
  "releaseDate": "2014-11-07T00:00:00Z",
  "rating": 8.7,
  "posterUrl": "https://example.com/interstellar.jpg"
}
```
* **Expected Status:** `201 Created`
* **Sample Response:**
```json
{
  "movieId": 1,
  "title": "Interstellar",
  "description": "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
  "genre": "Sci-Fi / Adventure",
  "language": "English",
  "durationMinutes": 169,
  "releaseDate": "2014-11-07T00:00:00Z",
  "rating": 8.7,
  "posterUrl": "https://example.com/interstellar.jpg",
  "isActive": true,
  "createdAt": "2026-11-02T04:34:00Z"
}
```

---

### 5.2 Get All Movies (Paged)
* **Method:** `GET`
* **URL:** `{{baseUrl}}/api/movies?page=1&pageSize=10`
* **Expected Status:** `200 OK`
* **Sample Response:**
```json
{
  "movies": [
    {
      "movieId": 1,
      "title": "Interstellar",
      "genre": "Sci-Fi / Adventure",
      "language": "English",
      "durationMinutes": 169,
      "rating": 8.7,
      "isActive": true
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

---

### 5.3 Search Movies By Title
* **Method:** `GET`
* **URL:** `{{baseUrl}}/api/movies/search?title=Interstellar`
* **Expected Status:** `200 OK`

---

## 🎟️ 6. Shows API

### 6.1 Create Show (Automatic ShowSeats Generation)
* **Method:** `POST`
* **URL:** `{{baseUrl}}/api/shows`
* **Header:** `Content-Type: application/json`
* **Body:**
```json
{
  "movieId": 1,
  "screenId": 1,
  "showDate": "2026-12-25T00:00:00Z",
  "startTime": "18:00:00",
  "basePrice": 200.00
}
```
* **Expected Status:** `201 Created`
* **Sample Response:**
```json
{
  "showId": 1,
  "movieId": 1,
  "movieTitle": "Interstellar",
  "durationMinutes": 169,
  "screenId": 1,
  "screenName": "Screen 1 - IMAX Laser",
  "theatreId": 1,
  "theatreName": "PVR INOX Grand",
  "showDate": "2026-12-25T00:00:00Z",
  "startTime": "18:00:00",
  "endTime": "20:49:00",
  "basePrice": 200.00,
  "isActive": true,
  "totalSeats": 6,
  "availableSeats": 6
}
```

---

### 6.2 Overlap Detection Test (Business Rule Verification)
* Try scheduling another show on the same screen that overlaps (e.g., StartTime `19:00:00` while the existing show runs until `20:49:00`):
```json
{
  "movieId": 1,
  "screenId": 1,
  "showDate": "2026-12-25T00:00:00Z",
  "startTime": "19:00:00",
  "basePrice": 200.00
}
```
* **Expected Status:** `400 Bad Request`
* **Sample Response:**
```json
{
  "statusCode": 400,
  "message": "A show already exists on screen 1 that overlaps with 19:00 - 21:49 on 2026-12-25.",
  "errorType": "InvalidOperationException",
  "timestamp": "2026-11-02T04:35:00Z"
}
```

---

### 6.3 Get Show By ID
* **Method:** `GET`
* **URL:** `{{baseUrl}}/api/shows/1`
* **Expected Status:** `200 OK`

---

## 💳 7. Bookings API

### 7.1 Create Booking
* **Method:** `POST`
* **URL:** `{{baseUrl}}/api/bookings`
* **Header:** `Content-Type: application/json`
* **Body:**
```json
{
  "showId": 1,
  "seatIds": [1, 2],
  "customerName": "Alice Johnson",
  "customerEmail": "alice@example.com",
  "customerPhone": "+91-9123456780"
}
```
* **Expected Status:** `201 Created`
* **Sample Response:**
```json
{
  "bookingId": 1,
  "showId": 1,
  "movieTitle": "Interstellar",
  "theatreName": "PVR INOX Grand",
  "screenName": "Screen 1 - IMAX Laser",
  "showDate": "2026-12-25T00:00:00Z",
  "startTime": "18:00:00",
  "customerName": "Alice Johnson",
  "customerEmail": "alice@example.com",
  "customerPhone": "+91-9123456780",
  "totalAmount": 360.00,
  "bookingStatus": "Confirmed",
  "bookingDate": "2026-11-02T04:36:00Z",
  "bookedSeats": [
    {
      "seatId": 1,
      "seatNumber": "A1",
      "seatType": "Regular",
      "price": 180.00
    },
    {
      "seatId": 2,
      "seatNumber": "A2",
      "seatType": "Regular",
      "price": 180.00
    }
  ]
}
```

---

### 7.2 Prevent Double-Booking Test (Business Rule Verification)
* Try booking the same seats (`1` or `2`) again for Show 1.
* **Expected Status:** `400 Bad Request`
* **Sample Response:**
```json
{
  "statusCode": 400,
  "message": "The following seats are not available: 1, 2",
  "errorType": "InvalidOperationException",
  "timestamp": "2026-11-02T04:37:00Z"
}
```

---

### 7.3 Get Booking By ID
* **Method:** `GET`
* **URL:** `{{baseUrl}}/api/bookings/1`
* **Expected Status:** `200 OK`

---

### 7.4 Cancel Booking
* **Method:** `POST`
* **URL:** `{{baseUrl}}/api/bookings/1/cancel`
* **Expected Status:** `200 OK`
* **Sample Response:**
```json
{
  "bookingId": 1,
  "bookingStatus": "Cancelled",
  "message": "Booking 1 has been successfully cancelled. 2 seat(s) are now available again.",
  "cancelledAt": "2026-11-02T04:38:00Z"
}
```

---

### 7.5 Verify Seats Restored
* Query `GET {{baseUrl}}/api/shows/1`.
* Notice `availableSeats` has returned back up by 2, and the seats can now be re-booked!
