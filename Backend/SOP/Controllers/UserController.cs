using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using SOP.Archive.DTOs;
using SOP.DTOs;
using SOP.Encryption;
using SOP.Entities;

namespace SOP.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;

        // Initializes the controller with the address repository
        public UserController(IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
        }

        [Authorize("Admin" , "Drift", "Instruktør")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<User> user = await _userRepository.GetAllAsync();

                List<UserResponse> userResponses = user.Select(
                    user => MapUserToUserResponse(user)).ToList();
                return Ok(userResponses);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør" , "Drift")]
        [HttpGet]
        [Route("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                List<User> users = await _userRepository.GetUsersByRoleAsync(3);

                List<UserResponse> userResponses = users.Select(user => MapUserToUserResponsePublic(user)).ToList();

                return Ok(userResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message); 
            }
        }


        [Authorize("Admin", "Instruktør", "Drift","Elev")]
        //[AllowAnonymous]
        [HttpGet]
        [Route("GetUsersByRoleId/{Id}")]
        public async Task<IActionResult> GetUsersByRole([FromRoute] int Id)
        {
            try
            {
                List<User> users = await _userRepository.GetUsersByRoleAsync(Id);

                List<UserResponse> userResponses = users.Select(user => new UserResponse
                {
                    Id = user.Id,
                    RoleId = user.RoleId,
                    Email = EncryptionHelper.Decrypt(user.Email),
                    Name = user.Name
                }).ToList();

                return Ok(userResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør", "Drift")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserRequest userRequest)
        {
            try
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt(10);

                User newUser = new User
                {
                    Email = EncryptionHelper.Encrypt(userRequest.Email),
                    Name = userRequest.Name,
                    Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, salt, true, BCrypt.Net.HashType.SHA512),
                    RoleId = userRequest.RoleId,
                    TwoFactorAuthentication = userRequest.TwoFactorAuthentication,
                    TwoFactorSecretKey = string.Empty,
                };


                var user = await _userRepository.CreateAsync(newUser);

                UserResponse userResponse = MapUserToUserResponsePublic(user);

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin" , "Instruktør" , "Drift" , "Elev")] 
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int Id)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(Id);
                if (user == null)
                {
                    return NotFound(); //Status Code 404
                }

                return Ok(MapUserToUserResponse(user));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør", "Drift", "Elev")]
        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int Id, [FromBody] UserRequest userRequest)
        {
            try
            {
                var existingUser = await _userRepository.FindByIdAsync(Id);
                if (existingUser == null) return NotFound();

                User updatedUser = new User
                {
                    Email = string.IsNullOrEmpty(userRequest.Email)
                        ? existingUser.Email
                        : EncryptionHelper.Encrypt(userRequest.Email),
                    Name = userRequest.Name,
                    RoleId = userRequest.RoleId,
                    TwoFactorAuthentication = userRequest.TwoFactorAuthentication,
                };

                var user = await _userRepository.UpdateByIdAsync(Id, updatedUser);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(MapUserToUserResponse(user));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }



        [Authorize("Admin", "Instruktør", "Drift", "Elev")]
        [HttpPut]
        [Route("updatePassword/{Id}")]
        public async Task<IActionResult> UpdatePasswordByIdAsync([FromRoute] int Id, [FromBody] UserRequest userRequest)
        {
            try
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt(10);

                User updateUserdPassword = new User
                {
                    // https://github.com/BcryptNet/bcrypt.net/issues/15
                    // The 3rd parameter is enhanced entropy which runs the password through SHA384 before using BCrypt hashing. Very secure!!!
                    // Double encryption!!
                    // Hashes the password from the request
                    Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, salt, true, BCrypt.Net.HashType.SHA512),
                };


                var user = await _userRepository.UpdatePasswordByIdAsync(Id, updateUserdPassword);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(MapUserToUserResponse(user));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize("Admin", "Instruktør", "Drift")]
        [HttpDelete]
        [Route("ArchiveById/{Id}")]
        public async Task<IActionResult> ArchiveByIdAsync([FromRoute] int Id, [FromBody] ArchiveNoteRequest archiveNoteRequest)
        {
            try
            {
                string archiveNote = archiveNoteRequest.ArchiveNote;
                var user = await _userRepository.ArchiveByIdAsync(Id, archiveNote);
                if (user == null)
                {
                    return NotFound();
                }

                Archive_UserResponse response = new Archive_UserResponse
                {
                    Id = user.Id,
                    DeleteTime = user.DeleteTime,
                    Email = EncryptionHelper.Decrypt(user.Email),
                    Name = user.Name,
                    Password = user.Password,
                    RoleId = user.RoleId,
                    TwoFactorAuthentication = user.TwoFactorAuthentication,
                    ArchiveNote = user.ArchiveNote,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest login)
        {
            try
            {
                // Encrypt the email before searching!
                string encryptedEmail = EncryptionHelper.Encrypt(login.Email);

                User user = await _userRepository.GetByEmail(encryptedEmail);
                if (user == null)
                {
                    return Unauthorized();
                }

                // BCrypt verification
                bool PasswordCheck = BCrypt.Net.BCrypt.Verify(login.Password, user.Password, true, BCrypt.Net.HashType.SHA512);

                if (PasswordCheck)
                {
                    SignInResponse SignInResponse = new()
                    {
                        Id = user.Id,
                        Token = _jwtUtils.GenerateJwtToken(user)
                    };
                    if (user.Role != null)
                    {
                        SignInResponse.Role = user.Role;
                    }
                    return Ok(SignInResponse);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private static UserResponse MapUserToUserResponse(User user)
        {
            UserResponse response = new UserResponse
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Email = EncryptionHelper.Decrypt(user.Email),
                Name = user.Name,
                Password = user.Password,
                TwoFactorAuthentication = user.TwoFactorAuthentication,
                UserRole = new UserRoleResponse
                {
                    Id = user.Role.Id,
                    Description = user.Role.Description,
                    Name = user.Role.Name,
                },
                UserLoans = user.Loans.Select(loan => new UserLoanResponse
                {
                    Id = loan.Id,
                    ItemId = loan.ItemId,
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate,
                    UserLoanItem = new UserLoanItemResponse
                    {
                        Id = loan.Item.Id,
                        ItemGroupId = loan.Item.ItemGroupId,
                        RoomId = loan.Item.RoomId,
                        SerialNumber = loan.Item.SerialNumber,
                        UserLoanItemItemGroup = new UserLoanItemItemGroupResponse
                        {
                            Id = loan.Item.ItemGroup.Id,
                            ItemTypeId = loan.Item.ItemGroup.ItemTypeId,
                            Manufacturer = loan.Item.ItemGroup.Manufacturer,
                            ModelName = loan.Item.ItemGroup.ModelName,
                            Price = loan.Item.ItemGroup.Price,
                            Quantity = loan.Item.ItemGroup.Quantity,
                            WarrantyPeriod = loan.Item.ItemGroup.WarrantyPeriod
                        }
                    }
                }).ToList()
            };
            return response;
        }


        private static User MapUserRequestToUser(UserRequest userRequest)
        {
            return new User
            {
                RoleId = userRequest.RoleId,
                Email = userRequest.Email,
                Name = userRequest.Name,
                Password = userRequest.Password,
                TwoFactorAuthentication = userRequest.TwoFactorAuthentication,
            };
        }

        public static UserResponse MapUserToUserResponsePublic(User user)
        {
            UserResponse response = new UserResponse
            {
                Id = user.Id,
                RoleId = user.RoleId,
                Email = EncryptionHelper.Decrypt(user.Email),
                Name = user.Name,
                Password = user.Password,
                TwoFactorAuthentication = user.TwoFactorAuthentication,
            };

            if (user.Role != null)
            {
                response.UserRole = new UserRoleResponse
                {
                    Id = user.Role.Id,
                    Description = user.Role.Description,
                    Name = user.Role.Name,
                };
            }

            // Er ikke helt sikker på det her. Skal den navigere hele vejen igennem ItemGroup?
            if (user.Loans != null)
            {
                response.UserLoans = user.Loans.Select(LoanItems => new UserLoanResponse
                {
                    Id = LoanItems.Id,
                    ItemId = LoanItems.ItemId,
                    LoanDate = LoanItems.LoanDate,
                    ReturnDate = LoanItems.ReturnDate,
                }).ToList();
            }

            return response;
        }


        // Not working yet. Need to implement 2FA
        /*
        [AllowAnonymous]
        [HttpPost]
        [Route("/Generate2FACode")]
        public async Task<IActionResult> Generate2FACode([FromBody] SignInRequest login)
        {
            var user = await _userRepository.GetByEmail(login.Email);
            if (user == null) return Unauthorized();

            // Generate a new secret key
            var secretKey = KeyGeneration.GenerateRandomKey(20); // 20 bytes = 160 bits
            var base32Secret = Base32Encoding.ToString(secretKey);

            // Store this key securely for the user (e.g., in the database)
            user.TwoFactorSecretKey = base32Secret;
            await _userRepository.UpdateByIdAsync(user.Id ,user);

            // Generate a QR code URI
            var qrCodeUri = GenerateQrCodeUri(user.Email, base32Secret);

            return Ok(new { qrCodeUri, base32Secret });
        }
        private string GenerateQrCodeUri(string email, string secretKey)
        {
            return $"otpauth://totp/SOP:{email}?secret={secretKey}&issuer=SOP&digits=6";
        }

        [HttpPost("api/auth/validate-2fa")]
        public async Task<IActionResult> Validate2FACode([FromBody] ValidateTwoFACodeRequest model)
        {
            var user = await _userRepository.GetByEmail(model.Email);
            if (user == null) return Unauthorized();

            var secretKey = user.TwoFactorSecretKey;
            if (string.IsNullOrEmpty(secretKey))
                return BadRequest("2FA is not set up for this user.");

            // Decode the secret and verify the TOTP code
            var otp = new Totp(Base32Encoding.ToBytes(secretKey));
            var expectedCode = otp.ComputeTotp();
            Console.WriteLine($"Secret: {secretKey}, Input: {model.TwoFactorAuthenticationCode}, Expected: {expectedCode}");

            var isValid = otp.VerifyTotp(model.TwoFactorAuthenticationCode, out long timeStepMatched, new VerificationWindow(2, 2));

            if (!isValid)
            {
                Console.WriteLine($"Verification failed. Matched Time Step: {timeStepMatched}");
                return Unauthorized("Invalid 2FA code.");
            }

            // If valid, continue with login or other actions
            return Ok("2FA verification successful.");
        }
        */
    }
}