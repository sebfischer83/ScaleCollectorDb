import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-auth-button',
  templateUrl: './auth-button.component.html',
  styleUrls: ['./auth-button.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthButtonComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
