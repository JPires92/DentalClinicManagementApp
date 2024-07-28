# Dental ClinicManagement Application
## Description
This application was developed as part of a 100H Full Stack Developer training. This allows you to manage appointments at a Dental Clinic.

## Features of application
### 1. Professional Team Area
**1.1. List professional team members. Filter by name and position.**

**1.2. Create/Edit/View/Delete a team member.**

**1.3. View a doctor's appointment history.**

**1.4. View a doctor's calendar of upcoming appointments.**


### 2. Customer Area
**2.1. List customers. Filter by name.**

**2.2. Create/Edit/View/Delete a customer.**

**2.3. View customer inquiry history.**

**2.4. See future queries from a customer.**


### 3. Appointment
**3.1. List of appointments. Filter by appointment number, client or doctor.**

**3.2. Create/Edit/View/Delete an appointment.**


### 4. Invoices
**4.1. List the invoices. Filter by invoice number, customer or date.**

**4.2. Create/Edit/View/Delete an invoice.**

**4.3. Indicate the total amount to be received.**

**4.4. Indicate the amount received per month.**

## Entity-Relationship Diagram of the application
![DentalClinicManagementApp_DER](https://github.com/user-attachments/assets/7472ef2e-44a5-4509-9c91-d9ae78824320)

## Run the application
This .NET application uses Entity Framework to generate the database schema.
For this, run the code below in the 'Package Manager Console'
### Using the Visual Studio
```properties 
Add-Migration InitialMigration
Update-Database
```
After that, you need to update the database connection string on the `appsettings.json` file.

When running the application, default users are created using seeds to use the application.

### Users created by seeds
| Username   |   Password   |
| :---       |       ---:   |
| admin      | admin2023    |
| worker     | worker2023   |

