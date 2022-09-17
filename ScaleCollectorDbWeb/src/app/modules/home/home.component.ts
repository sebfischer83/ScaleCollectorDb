import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  weatherItems: any = [];
  constructor(public auth: AuthService, private http: HttpClient) {}

  ngOnInit(): void {
    this.http
      .get('http://localhost:5000/WeatherForecast')
      .subscribe((reply) => {
        this.weatherItems = reply;
      });
  }
}
