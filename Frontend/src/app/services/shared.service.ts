import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private computerId: number | null = null;

  setComputerId(id: number): void {
    this.computerId = id;
  }

  getComputerId(): number | null {
    return this.computerId;
  }

  clearComputerId(): void {
    this.computerId = null;
  }
}