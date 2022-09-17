import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { CoreModule } from './core/core.module';
import { HomeModule } from './modules/home/home.module';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_GB } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { IconsProviderModule } from './icons-provider.module';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { LandingPageComponent } from './pages/landing-page/landing-page.component';
import { LandingPageContentComponent } from './pages/landing-page-content/landing-page-content.component';
import { MainLayoutComponent } from './pages/main-layout/main-layout.component';

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    LandingPageComponent,
    LandingPageContentComponent,
    MainLayoutComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CoreModule,
    HomeModule,
    FormsModule,
    BrowserAnimationsModule,
    IconsProviderModule,
    NzLayoutModule,
    NzMenuModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    { provide: NZ_I18N, useValue: en_GB },
  ],
  bootstrap: [AppComponent],
  exports: [],
})
export class AppModule {}
