// will user https on production environment
export const COOKIE_OPTIONS = { httpOnly: false, secure: import.meta.env.PROD };

export const LOGIN_PATH = '/login';
export const REGISTER_PATH = '/register';
export const LOGOUT_PATH = '/logout';
export const FORGOT_PASSWORD_PATH = '/forgot-password';
export const supportLanguages: Record<string, string> = {
    'en-US': 'English',
    'de-DE': 'Deutsch',
    'zh-CN': '中文'
};