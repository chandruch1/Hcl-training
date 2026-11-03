import { Routes } from '@angular/router';
import { MovieListComponent } from './pages/movie-list/movie-list';
import { MovieDetailComponent } from './pages/movie-detail/movie-detail';
import { TheatresComponent } from './pages/theatres/theatres';
import { ShowsComponent } from './pages/shows/shows';
import { BookingComponent } from './pages/booking/booking';
import { BookingConfirmationComponent } from './pages/booking-confirmation/booking-confirmation';
import { MyBookingsComponent } from './pages/my-bookings/my-bookings';
import { LoginComponent } from './pages/auth/login/login';
import { RegisterComponent } from './pages/auth/register/register';
import { AdminLogin } from './pages/admin-login/admin-login';
import { AdminDashboard } from './pages/admin-dashboard/admin-dashboard';
import { adminGuard } from './guards/admin.guard';

export const routes: Routes = [
  {
    path: '',
    component: MovieListComponent
  },
  {
    path: 'movies',
    component: MovieListComponent
  },
  {
    path: 'movies/:id',
    component: MovieDetailComponent
  },
  {
    path: 'theatres',
    component: TheatresComponent
  },
  {
    path: 'shows',
    component: ShowsComponent
  },
  {
    path: 'booking/:showId',
    component: BookingComponent
  },
  {
    path: 'booking-confirmation/:bookingId',
    component: BookingConfirmationComponent
  },
  {
    path: 'my-bookings',
    component: MyBookingsComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'admin/login',
    component: AdminLogin
  },
  {
    path: 'admin/dashboard',
    component: AdminDashboard,
    canActivate: [adminGuard]
  },
  {
    path: '**',
    redirectTo: ''
  }
];
