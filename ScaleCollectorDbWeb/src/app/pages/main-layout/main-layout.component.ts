import { Component, OnInit } from '@angular/core';
import { ModelKitsService } from 'src/api';
import { ThemeService } from 'src/app/theme.service';

@Component({
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss'],
})
export class MainLayoutComponent implements OnInit {
  isCollapsed = false;
  constructor(
    private themeService: ThemeService,
    private s: ModelKitsService
  ) {}

  ngOnInit(): void {
    console.log('MainLayoutComponent');
  }

  toggleTheme(): void {
    this.themeService.toggleTheme().then();
  }
}
