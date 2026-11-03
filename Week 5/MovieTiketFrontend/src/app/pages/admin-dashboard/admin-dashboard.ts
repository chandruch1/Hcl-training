import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboard {

  private router = inject(Router);

  adminName = sessionStorage.getItem('adminName') || 'Admin';

  logout() {
    sessionStorage.clear();
    this.router.navigate(['/admin/login']);
  }
}