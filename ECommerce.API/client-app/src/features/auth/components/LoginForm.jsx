import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../authService';
import { toast } from 'react-toastify';

const LoginForm = () => {
  const [form, setForm] = useState({ email: '', password: '' });
  const navigate = useNavigate();

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await login(form);
      toast.success('Login successful');
      navigate('/');
    } catch (err) {
      toast.error('Invalid email or password');
    }
  };

  return (
    <form onSubmit={handleSubmit} className="card p-4 shadow-sm">
      <div className="mb-3">
        <label>Email</label>
        <input
          type="email"
          name="email"
          className="form-control"
          value={form.email}
          onChange={handleChange}
          required
        />
      </div>
      <div className="mb-3">
        <label>Password</label>
        <input
          type="password"
          name="password"
          className="form-control"
          value={form.password}
          onChange={handleChange}
          required
        />
      </div>
      <button type="submit" className="btn btn-primary w-100">Login</button>
    </form>
  );
};

export default LoginForm;
