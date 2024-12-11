using ChatHub.Application.IRepository;
using ChatHub.Application;
using ChatHub.Application.IService;
using ChatHub.Domain.Entities;
using ChatHub.Global.Shared.Commons;
using ChatHub.Global.Shared.ViewModel.UserViewModel;
using ChatHub.Global.Shared.Helpers;

namespace ChatHub.Infrastructures.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository,
            IMessageRepository messageRepository,
            IJwtService jwtService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _messageRepository = messageRepository;
        }
        public async Task<ApiResult<string>> AuthenCate(LoginRequestModel request)
        {
            var token = new Global.Shared.ViewModel.TokenViewModel.TokenObjectModel();
            if (!string.IsNullOrEmpty(request.UserName))
            {
                var user = await _userRepository.GetByUsernameAsync(request.UserName);
                if (user == null)
                {
                    return new ApiErrorResult<string>(Constant.INVALID_USERNAME_PASSWORD);
                }
                else if (user.IsLockedOut == true)
                {
                    return new ApiErrorResult<string>(Constant.ACOUNT_IS_LOCK);
                }
                var passwordVerificationResult = PasswordHasher.Compare(request.PassWord, user.HashedPassword!);
                if (passwordVerificationResult != PasswordVerificationResult.Success)
                    return new ApiErrorResult<string>(Constant.INVALID_USERNAME_PASSWORD);
                token = _jwtService.GenerateToken(user);
            }
            return new ApiSuccessResult<string>(token.Token);
        }

        public async Task<ApiResult<List<GetAllUserResponeModel>>> GetAllUserChatWithUserLogin(Guid loginId)
        {
            var result = new List<GetAllUserResponeModel>();
            var listUser = await _userRepository.GetAllAsync();
            var dicMessage = await _messageRepository.GetMessageByLoginIdAsync(loginId);
            foreach (var user in listUser) 
            {
                
                var userResult = new GetAllUserResponeModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Phone = user.Phone,
                }; 
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Image\\Avatar", user.Avartar ?? string.Empty);
                if (File.Exists(path))
                {
                    // Read the image file and convert it to a base64 string
                    var imageBytes = await File.ReadAllBytesAsync(path);
                    var base64String = Convert.ToBase64String(imageBytes);

                    // Optionally, include the MIME type in the base64 string
                    var fileExtension = Path.GetExtension(path).ToLowerInvariant();
                    string mimeType = fileExtension switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".png!w700wp" => "image/png!w700wp",
                        _ => "application/octet-stream", // Default MIME type
                    };
                    userResult.Avartar = $"data:{mimeType};base64,{base64String}";
                }
                if(dicMessage != null)
                {
                    var lastMessage = dicMessage.Where(x => x.Key == userResult.Id).Select(x => x.Value).FirstOrDefault();
                    if (lastMessage != null) 
                    {
                        userResult.LastMessage = lastMessage.Content;
                        var totalHour = (int)DateTime.Now.Subtract(lastMessage.ModifiDateMessage!.Value).TotalHours;
                        if (totalHour > 0 && totalHour <=24)
                        {
                            userResult.TimeMessageSendd = (int)DateTime.Now.Subtract(lastMessage.ModifiDateMessage!.Value).TotalHours;
                            userResult.IsHour = true;
                        }
                        else if(totalHour == 0)
                        {
                            userResult.TimeMessageSendd = (int)DateTime.Now.Subtract(lastMessage.ModifiDateMessage!.Value).TotalMinutes;
                            userResult.IsHour = false;
                        }
                        else
                        {
                            userResult.IsDay = true;
                        }
                        userResult.ModifileDateMessage = lastMessage.ModifiDateMessage;
                    }
                }
                result.Add(userResult);
            }
            result = result.OrderByDescending(x => x.ModifileDateMessage).ToList();
            return new ApiSuccessResult<List<GetAllUserResponeModel>>(result);
        }

        public Task<ApiResult<User>> GetByID(Guid ID)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<UserResponseModel>> GetByUserName(string userName)
        {
            if(userName!= null)
            {
                var avartar = string.Empty;
                var user = await _userRepository.GetByUsernameAsync(userName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Image\\Avatar", user.Avartar ?? string.Empty);
                if (File.Exists(path))
                {
                    // Read the image file and convert it to a base64 string
                    var imageBytes = await File.ReadAllBytesAsync(path);
                    var base64String = Convert.ToBase64String(imageBytes);

                    // Optionally, include the MIME type in the base64 string
                    var fileExtension = Path.GetExtension(path).ToLowerInvariant();
                    string mimeType = fileExtension switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".png!w700wp" => "image/png!w700wp",
                        _ => "application/octet-stream", // Default MIME type
                    };
                    avartar = $"data:{mimeType};base64,{base64String}";

                }

                return new ApiSuccessResult<UserResponseModel>(new UserResponseModel
                {
                Id = user.Id,
                UserName = userName,
                Phone = user.Phone,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avartar = avartar,
                });
            }
            return new ApiErrorResult<UserResponseModel>(Constant.GET_USE_BY_NAME_FAIL);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequestModel request)
        {
            if (request != null)
            {
                var user = await _userRepository.GetByUsernameAsync(request.UserName ?? string.Empty);
                if (user != null)
                {
                    return new ApiErrorResult<bool>(Constant.USER_ALREADY_EXISTS);
                }
                else
                {
                    
                    if (request.File != null && request.File.Length > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "Image\\Avatar");
                        var fileName = request.UserName + "_" + request.File.FileName;
                        var filePath = Path.Combine(path, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await request.File.CopyToAsync(stream);
                        }
                    }

                    var appUser = new User
                    {
                        UserName = request.UserName,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Phone = request.PhoneNumber,
                        HashedPassword = PasswordHasher.HashPassword(request.PassWord ?? string.Empty),
                        Dob = request.Dob,
                        Avartar = request.UserName + "_" + request.File?.FileName
                    };
                    await _userRepository.AddAsync(appUser);
                    var isSave = await _unitOfWork.SaveChangeAsync();
                    if (isSave == 1)
                    {
                        return new ApiSuccessResult<bool>();
                    }
                }
            }
            return new ApiErrorResult<bool>(Constant.USER_REGISTER_FAIL);
        }
    }
}
