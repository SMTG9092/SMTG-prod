using Supabase;
using SMTG.API.DTOs;
using Dapper;
using System.Linq;

namespace SMTG.API.Services;

public class AuthService
{
    private readonly SupabaseService _supabase;
    private readonly JwtService _jwt;
    private readonly DatabaseService _databaseService;

    public AuthService(
        SupabaseService supabase,
        JwtService jwt,
        DatabaseService databaseService)
    {
        _supabase = supabase;
        _jwt = jwt;
        _databaseService = databaseService;
    }

    public async Task<LoginResponse> Login(string email, string password)
    {
        try
        {
            // 1. Authenticate user using Supabase Auth
            var session = await _supabase.Client.Auth.SignIn(email, password);

            if (session?.User == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Email ou mot de passe incorrect."
                };
            }

            var userIdGuid = Guid.Parse(session.User.Id);
            var userEmail = session.User.Email ?? email;
            string username = userEmail;
            string role = "USER";
            var pages = new List<string>();
            var actions = new List<string>();

            // 2, 3, 4, 5. Read user profile, role, pages, and actions using Dapper and DatabaseService
            using var connection = _databaseService.GetConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT
                    up.username,
                    r.code AS role_name
                FROM user_profiles up
                LEFT JOIN roles r
                ON r.id=up.role_id
                WHERE up.id=@UserId;

                SELECT
                    p.code
                FROM role_page_permissions rpp
                JOIN user_profiles up
                ON up.role_id=rpp.role_id
                JOIN pages p
                ON p.id=rpp.page_id
                WHERE up.id=@UserId
                ORDER BY p.ordre_affichage;

                SELECT
                    a.code
                FROM role_page_actions rpa
                JOIN page_actions pa
                ON pa.id=rpa.page_action_id
                JOIN actions a
                ON a.id=pa.action_id
                JOIN user_profiles up
                ON up.role_id=rpa.role_id
                WHERE up.id=@UserId
                ORDER BY a.code;
            ";

            using var multi = await connection.QueryMultipleAsync(sql, new { UserId = userIdGuid });

            var profileResult = await multi.ReadAsync<UserProfileDto>();
            var profile = profileResult.FirstOrDefault();

            if (profile != null)
            {
                if (!string.IsNullOrEmpty(profile.Username))
                {
                    username = profile.Username;
                }
                if (!string.IsNullOrEmpty(profile.RoleName))
                {
                    role = profile.RoleName;
                }
            }

            pages = (await multi.ReadAsync<string>()).Where(p => !string.IsNullOrEmpty(p)).ToList();
            actions = (await multi.ReadAsync<string>()).Where(a => !string.IsNullOrEmpty(a)).ToList();

            // 6. Generate local JWT using JwtService
            var token = _jwt.GenerateToken(
                session.User.Id,
                userEmail,
                username,
                role
            );

            // 7. Return LoginResponse
            return new LoginResponse
            {
                Success = true,
                Message = "Connexion réussie.",
                Token = token,
                UserId = session.User.Id,
                Email = userEmail,
                Username = username,
                Role = role,
                Pages = pages,
                Actions = actions
            };
        }
        catch (Exception ex)
    {
         return new LoginResponse
        {
        Success = false,
        Message = ex.ToString()
        };
    }
    }

    private class UserProfileDto
    {
        public string? Username { get; set; }
        public string? RoleName { get; set; }
    }
}