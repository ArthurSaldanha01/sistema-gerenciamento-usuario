import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../service/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {
  email: string = '';
  message: string = '';
  error: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  async sendResetLink() {
    this.message = '';
    this.error = '';
    const success = await this.authService.forgotPassword(this.email);
    if (success) {
      this.message = 'Um link para redefinir a senha foi enviado para seu e-mail.';
    } else {
      this.error = 'Erro ao enviar o link. Tente novamente.';
    }
  }
}
