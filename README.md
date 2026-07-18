<p align="center">
  <img src="https://res.cloudinary.com/df6ylojjq/image/upload/v1736182255/devTalk_fxlk05.svg" alt="Devtalk Logo" width="200">
</p>

<h1 align="center">Devtalk</h1>

<p align="center">
  <b>A production-grade social platform for developers — built to prove what real backend engineering looks like.</b>
</p>

<p align="center">
  Clean Architecture · CQRS · Real-Time SignalR · Redis Caching · Full Observability Stack
</p>

---

## Why This Project Exists

Devtalk isn't a CRUD tutorial app. It's a backend engineering playground built to tackle the same problems production systems face at scale: real-time delivery to thousands of concurrent users, cache invalidation, observability, and clean, testable architecture. Every design decision below — from cursor pagination to distributed tracing — was made the way it would be on a real engineering team, not to check a box.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture & Technologies](#architecture--technologies)
- [Observability Stack](#observability-stack)
- [API Endpoints](#api-endpoints)
  - [Authentication](#authentication)
  - [Bookmarks](#bookmarks)
  - [Category](#category)
  - [Comment](#comment)
  - [Notifications](#notifications)
  - [Post](#post)
  - [Preferences](#preferences)
  - [Votes](#votes)
- [Performance Enhancements](#performance-enhancements)
- [Installation & Setup](#installation--setup)
- [Testing & CI/CD](#testing--cicd)
- [Contributing](#contributing)
- [License](#license)

## Overview

Devtalk is designed for developers who want to keep up with industry trends and share their experiences. It allows users to register, authenticate, create posts, comment on discussions, bookmark favorite content, vote on posts, and receive real-time notifications — all backed by an API built for scale and observability from day one.

## Features

- 🔐 **User Management:** Secure registration, login, password recovery, and email confirmation.
- 📝 **Content Creation:** Create, update, delete, and fetch posts with robust commenting and voting mechanisms.
- 🔖 **Bookmarks & Categories:** Organize content with bookmarks and categorize posts for a streamlined experience.
- ⚡ **Real-Time Notifications:** Instant updates via SignalR with an in-memory message queue for asynchronous operations.
- 🏗️ **Scalable Architecture:** Implemented using Clean Architecture and the CQRS pattern with the Mediator library.
- 📊 **Full-Stack Observability:** Distributed tracing and metrics via **OpenTelemetry**, visualized in **Grafana** dashboards powered by **Prometheus** — giving real production-grade insight into latency, throughput, and system health.
- 🧾 **Robust Logging & Monitoring:** Logging with Serilog, integrated with Azure Application Insights and ElasticSearch/Kibana for deep insights.
- 🚀 **Advanced Caching:** Redis caching with an event-driven cache invalidation strategy.
- 📄 **Efficient Pagination:** Cursor pagination for posts, offering significant performance improvements over offset pagination.

## Architecture & Technologies

- **Clean Architecture & CQRS:** The application is structured with a clear separation of concerns. The CQRS pattern, implemented via the Mediator library, ensures that commands and queries are handled efficiently.
- **Observability:** Instrumented end-to-end with **OpenTelemetry**, exporting metrics to **Prometheus** and visualized through custom **Grafana** dashboards — enabling real-time visibility into request latency, throughput, and error rates across the system.
- **Logging & Monitoring:** Utilizes Serilog for logging, with integrations to Azure Application Insights and ElasticSearch/Kibana for robust monitoring and diagnostics.
- **Caching:** Implements Redis for caching, coupled with an advanced event-driven cache invalidation mechanism to keep data fresh and responsive.
- **Real-Time Communication:** Leverages SignalR for real-time user notifications along with an in-memory message queue to process asynchronous operations.
- **Testing & CI/CD:** Testing is conducted with xUnit, and the project is integrated with continuous integration and delivery pipelines to ensure code quality and rapid deployment.

## Observability Stack

Devtalk ships with a full observability pipeline, not just logs:

- **OpenTelemetry** instruments the API to capture distributed traces and metrics across requests, including SignalR real-time flows.
- **Prometheus** scrapes and stores time-series metrics from the instrumented application.
- **Grafana** dashboards turn those metrics into actionable visuals — request rates, latency percentiles, error rates, and system health at a glance.

This means Devtalk isn't just monitored after something breaks — it's observable by design, the same way production services are.

## API Endpoints

### Authentication

- **Register:**  
  `POST /api/Auth/register`

- **Confirm Email:**  
  `GET /api/Auth/confirmEmail`

- **Resend Confirmation Email:**  
  `POST /api/Auth/resendConfirmationEmail`

- **Login:**  
  `POST /api/Auth/login`

- **Forget Password:**  
  `POST /api/Auth/forgetPassword`

- **Reset Password:**  
  `POST /api/Auth/{UserId}/{Token}/reset-password`

- **Refresh Token:**  
  `GET /api/Auth/refreshToken`

### Bookmarks

- **Get All Bookmarks:**  
  `GET /api/Bookmarks/all`

- **Get Specific Bookmark:**  
  `GET /api/Bookmarks/{bookmarkId}`

- **Add Bookmark (for a post):**  
  `POST /api/Bookmarks/{postId}`

- **Remove Bookmark:**  
  `DELETE /api/Bookmarks/{postId}`

### Category

- **Get All Categories:**  
  `GET /api/Category/all`

- **Get Category Details:**  
  `GET /api/Category/{categoryId}`

- **Delete Category:**  
  `DELETE /api/Category/{categoryId}`

- **Create Category:**  
  `POST /api/Category`

### Comment

- **Add Comment to a Post:**  
  `POST /api/{PostId}/Comment`

- **Get Comments for a Post:**  
  `GET /api/{PostId}/Comment`

- **Delete Comment:**  
  `DELETE /api/{PostId}/Comment/{CommentId}`

- **Update Comment:**  
  `PATCH /api/{PostId}/Comment/{CommentId}`

- **Get Specific Comment:**  
  `GET /api/{PostId}/Comment/{CommentId}`

### Notifications

- **Get All Notifications:**  
  `GET /api/Notifications/all`

- **Mark a Notification as Read:**  
  `POST /api/Notifications/{notificationId}/read`

- **Mark All Notifications as Read:**  
  `POST /api/Notifications/read`

### Post

- **Get All Posts:**  
  `GET /api/Post/all`

- **Get a Specific Post:**  
  `GET /api/Post/{PostId}`

- **Create a Post:**  
  `POST /api/Post/create`

- **Update a Post:**  
  `PATCH /api/Post/update`

- **Delete a Post:**  
  `DELETE /api/Post/delete/{id}`

- **Get Trending Posts:**  
  `GET /api/Post/trending`

- **User Feed:**  
  `GET /api/Post/feed`

- **Posts by Category:**  
  `GET /api/Post/category/{categoryId}`

### Preferences

- **Set Preference for a Category:**  
  `POST /api/Prefernces/{categoryId}`

- **Remove Preference:**  
  `DELETE /api/Prefernces/{categoryId}`

- **General Preferences Update:**  
  `POST /api/Prefernces`

### Votes

- **Upvote a Post:**  
  `POST /api/{PostId}/Votes/upvote`

- **Downvote a Post:**  
  `POST /api/{PostId}/Votes/downvote`

## Performance Enhancements

**Cursor Pagination vs. Offset Pagination:**  
Devtalk uses cursor pagination for posts, which is significantly more efficient than traditional offset pagination when handling large datasets. Cursor pagination reduces the performance overhead of large offset values by offering near constant-time retrieval regardless of the page depth. This results in faster load times and a more scalable solution for real-time data feeds.

**Real-Time at Scale:**  
The SignalR notification pipeline has been load-tested to handle high volumes of concurrent connections, with Redis backplane support for horizontal scaling across multiple server instances.

## Installation & Setup

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/fares7elsadek/DevTalk-Web-API
   cd devtalk
   ```

2. **Configure Environment Variables:**  
   Create a `.env` file based on the provided `.env.example` and set your connection strings and API keys for services like Azure Application Insights, ElasticSearch, Redis, Prometheus, etc.

3. **Install Dependencies:**

   ```bash
   dotnet restore
   ```

4. **Run the Application:**

   ```bash
   dotnet run
   ```

5. **Run the Observability Stack (Prometheus & Grafana):**

   ```bash
   docker-compose up -d prometheus grafana
   ```

   Grafana dashboards will be available at `http://localhost:3000`, with metrics scraped from the API's OpenTelemetry-instrumented endpoints via Prometheus.

6. **Running Tests:**

   ```bash
   dotnet test
   ```

## Testing & CI/CD

- **Testing:**  
  The project includes comprehensive unit and integration tests written with xUnit. These tests help ensure that both the business logic and API endpoints behave as expected.

- **CI/CD:**  
  The repository is integrated with a CI/CD pipeline which automatically runs tests, builds the project, and deploys updates. This ensures that new changes are validated and deployed quickly with minimal downtime.

## Contributing

Contributions are welcome! Please fork the repository and create a pull request for any improvements or bug fixes. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.