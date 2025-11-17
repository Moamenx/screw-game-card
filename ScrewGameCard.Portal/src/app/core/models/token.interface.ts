export interface JwtPayload {
  // Standard claims
    iss?: string;
    sub?: string;
    aud?: string[] | string;
    exp?: number;
    nbf?: number;
    iat?: number;
    jti?: string;
  userId?: string;     // Custom user ID claim
  fullName?: string;   // User's full name
  avatar?: string;    // Profile picture URL
  // Add any other custom claims you have
}