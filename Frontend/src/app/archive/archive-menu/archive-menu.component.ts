import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../../navbar/navbar.component';

@Component({
  selector: 'app-archive-menu',
  imports: [CommonModule, FormsModule, NavbarComponent, RouterModule],
  templateUrl: './archive-menu.component.html',
  styleUrl: './archive-menu.component.css',
})
export class ArchiveMenuComponent {}
