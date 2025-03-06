import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../service/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  email: string = '';
  token: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  message: string = '';
  error: string = '';

  constructor(private route: ActivatedRoute, private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'] || '';
      this.token = params['token'] || '';
    });
  }

  async resetPassword() {
    this.message = '';
    this.error = '';
    if (this.newPassword !== this.confirmPassword) {
      this.error = "As senhas não coincidem.";
      return;
    }
    const success = await this.authService.resetPassword(this.email, this.token, this.newPassword);
    if (success) {
      this.message = "Senha redefinida com sucesso! Redirecionando para o login...";
      setTimeout(() => {
        this.router.navigate(['/login']);
      }, 3000);
    } else {
      this.error = "Erro ao redefinir a senha.";
    }
  }
}
