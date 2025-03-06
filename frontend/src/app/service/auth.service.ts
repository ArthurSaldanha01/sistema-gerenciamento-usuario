import { Injectable } from '@angular/core';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, Observable } from 'rxjs';

export interface User {
  email: string;
  userName: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiRegisterUrl = 'http://localhost:5000/api/users/register';
  private apiLoginUrl = 'http://localhost:5000/api/auth/login';

  private currentUserSubject: BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);
  public currentUser$: Observable<User | null> = this.currentUserSubject.asObservable();

  async register(email: string, password: string, role: string = 'user'): Promise<boolean> {
    try {
      console.log('Enviando para API:', { email, password, role });
      const response = await axios.post(
        this.apiRegisterUrl,
        { email, password, role },
        { headers: { 'Content-Type': 'application/json' } }
      );
      console.log('Usuário cadastrado com sucesso!', response.data);
      return true;
    } catch (error: any) {
      console.error('Erro ao cadastrar:', error.response ? error.response.data : error.message);
      return false;
    }
  }

  async login(email: string, password: string): Promise<boolean> {
    try {
      const response = await axios.post(this.apiLoginUrl, { email, password });
      localStorage.setItem('token', response.data.token);
      await this.loadUser();
      return true;
    } catch (error) {
      console.error('Erro ao fazer login:', error);
      return false;
    }
  }

  async loadUser(): Promise<void> {
    const token = this.getToken();
    if (!token) {
      this.currentUserSubject.next(null);
      return;
    }
    try {
      const response = await axios.get('http://localhost:5000/api/users/me', {
        headers: { Authorization: `Bearer ${token}` }
      });
      const user: User = {
        email: response.data.email,
        userName: response.data.userName,
      };
      this.currentUserSubject.next(user);
    } catch (error) {
      console.error('Erro ao carregar usuário:', error);
      this.logout();
    }
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;
    try {
      const decoded: any = jwtDecode(token);
      return decoded.exp * 1000 > Date.now();
    } catch {
      return false;
    }
  }

  async forgotPassword(email: string): Promise<boolean> {
    try {
      await axios.post('http://localhost:5000/api/auth/forgot-password', { Email: email });
      return true;
    } catch (error) {
      console.error("Erro no forgotPassword", error);
      return false;
    }
  }

  async resetPassword(email: string, token: string, newPassword: string): Promise<boolean> {
    try {
      await axios.post('http://localhost:5000/api/auth/reset-password', {
        Email: email,
        Token: token,
        NewPassword: newPassword
      });
      return true;
    } catch (error) {
      console.error("Erro no resetPassword", error);
      return false;
    }
  }

}
