import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService, User } from '../../service/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  // Usamos o observable para pegar os dados do usuário
  currentUser$: Observable<User | null>;

  constructor(public authService: AuthService, public router: Router) {
    this.currentUser$ = this.authService.currentUser$;
  }

  async ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    // Garante que os dados do usuário sejam carregados
    await this.authService.loadUser();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
