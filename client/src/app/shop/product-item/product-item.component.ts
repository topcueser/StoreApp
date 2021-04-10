import { Component, Input, OnInit } from '@angular/core';
import { IProduct } from 'src/app/models/product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})

export class ProductItemComponent implements OnInit {

  @Input() product: IProduct; // component icine deger tasimak icin olusturuldu. shop.component icinden buraya deger gonderiliyor

  constructor() { }

  ngOnInit(): void {
  }

}
