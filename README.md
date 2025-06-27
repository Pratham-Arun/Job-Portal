# Job Portal

An ASP.NET MVC web application for job posting, resume screening, and applicant management. The system supports multiple user roles (Admin, Employer, Applicant) and provides automated skill matching for job applications.

## Features

- **User Roles:**
  - **Admin:** Manage users, view all jobs, view employer/applicant profiles, manage skills.
  - **Employer:** Post, edit, and delete jobs; view applicants and their profiles; manage company profile.
  - **Applicant:** Register, edit profile, search and apply for jobs, view application status.
- **Job Management:** Employers can specify company name, salary range (in rupees), experience range, employment type (remote/hybrid/on-site), job type (part-time/full-time/internship), and number of vacancies.
- **Profile Management:** Applicants and employers can edit their profiles.
- **Password Reset:** Users can reset forgotten passwords.

## Getting Started

### Prerequisites
- [.NET 6 SDK or later](https://dotnet.microsoft.com/download)
- [SQLite](https://www.sqlite.org/download.html) (or use the included `app.db`)
- Entity Framework Core Tools

### Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Pratham-Arun/Job-Portal.git
   cd Job-Portal
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Apply database migrations:**
   ```bash
   dotnet tool install --global dotnet-ef # if not already installed
   dotnet ef database update
   ```
   > If you encounter schema issues, delete `app.db` and re-run the migration commands above.

4. **Run the application:**
   ```bash
   dotnet run
   ```
   The app will be available at `https://localhost:5001` or `http://localhost:5000`.

## Usage

- Register as an Applicant or Employer.
- Employers can post jobs, specifying all required details.
- Applicants can edit their profiles and apply for jobs.
- Admins can manage users, jobs, and skills from the admin dashboard.
- The system automatically calculates skill match scores for applications.

## Project Structure

- `Controllers/` - MVC controllers for handling requests
- `Models/` - Entity models for EF Core
- `ViewModels/` - View models for forms and data transfer
- `Views/` - Razor views for UI
- `Migrations/` - EF Core migration files
- `wwwroot/` - Static files (CSS, JS, images)

## Contributing

1. Fork the repository
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -am 'Add new feature'`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.
