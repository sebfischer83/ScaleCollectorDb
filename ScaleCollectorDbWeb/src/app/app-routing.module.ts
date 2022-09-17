import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LandingPageContentComponent } from './pages/landing-page-content/landing-page-content.component';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';
import { MainLayoutComponent } from './pages/main-layout/main-layout.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/start' },
  {
    path: 'start',
    component: LandingPageComponent,
    children: [{ path: '', component: LandingPageContentComponent }],
  },
  {
    path: 'home',
    component: MainLayoutComponent,
    loadChildren: () =>
      import('./modules/home/home.module').then((m) => m.HomeModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
