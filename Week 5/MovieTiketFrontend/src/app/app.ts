
import { Component, signal, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiTest } from './services/api-test';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {

  private apiTest = inject(ApiTest);

  protected readonly title = signal('Movie Ticket Booking');

  protected readonly loading = signal(false);
  protected readonly response = signal('');
  protected readonly errorMessage = signal('');

  testConnection() {

    this.loading.set(true);
    this.response.set('');
    this.errorMessage.set('');

    this.apiTest.testConnection().subscribe({

      next: (data) => {
        this.response.set(JSON.stringify(data, null, 2));
        this.loading.set(false);
      },

      error: (error) => {
        this.errorMessage.set(
          error.message || 'Unable to connect to backend'
        );
        this.loading.set(false);
      }

    });
  }
}
