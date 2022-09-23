import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { KitStatus, KitType, ModelKitsService } from 'src/api';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  constructor(public auth: AuthService, private modelkits: ModelKitsService) {}

  ngOnInit(): void {}

  test() {
    this.modelkits
      .apiModelKitsPost(
        {
          brandId: 1,
          scaleId: 1,
          status: KitStatus.Stash,
          type: KitType.Kit,
          title: 'test',
          manufacturerArticleNumber: '22334',
        },
        undefined,
        undefined
      )
      .subscribe((x) => {
        console.log(JSON.stringify(x));
      });
  }
}
