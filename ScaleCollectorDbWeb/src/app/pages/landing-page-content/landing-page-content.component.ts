import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  templateUrl: './landing-page-content.component.html',
  styleUrls: ['./landing-page-content.component.scss'],
})
export class LandingPageContentComponent implements OnInit {
  constructor(public auth: AuthService) {}

  ngOnInit(): void {}
}
