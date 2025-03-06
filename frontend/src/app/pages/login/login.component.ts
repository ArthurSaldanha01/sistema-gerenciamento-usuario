import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../service/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  email = '';
  password = '';

  constructor(private authService: AuthService, private router: Router) {}

  async login() {
    const success = await this.authService.login(this.email, this.password);
    if (success) {
      this.router.navigate(['/dashboard']);
    } else {
      alert('Login inválido! Verifique suas credenciais.');
    }
  }
}
