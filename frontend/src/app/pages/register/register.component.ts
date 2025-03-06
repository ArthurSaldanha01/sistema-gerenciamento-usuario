import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../service/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  email = '';
  password = '';
  role = 'user';

  passwordValidations = {
    minLength: false,
    uppercase: false,
    lowercase: false,
    number: false,
    specialChar: false,
  };

  constructor(private authService: AuthService, private router: Router) {}

  validatePassword(): void {
    const pwd = this.password;
    this.passwordValidations.minLength = pwd.length >= 6;
    this.passwordValidations.uppercase = /[A-Z]/.test(pwd);
    this.passwordValidations.lowercase = /[a-z]/.test(pwd);
    this.passwordValidations.number = /[0-9]/.test(pwd);
    this.passwordValidations.specialChar = /[!@#$%^&*(),.?":{}|<>]/.test(pwd);
  }

  async register() {
    const allValid = Object.values(this.passwordValidations).every(val => val);
    if (!allValid) {
      alert('A senha não atende a todos os critérios de segurança.');
      return;
    }

    const success = await this.authService.register(this.email, this.password, this.role);
    if (success) {
      alert('Cadastro realizado com sucesso! Faça login.');
      this.router.navigate(['/login']);
    } else {
      alert('Erro ao cadastrar. Verifique os dados e tente novamente.');
    }
  }
}
