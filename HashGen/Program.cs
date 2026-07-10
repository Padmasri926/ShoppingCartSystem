using Microsoft.AspNetCore.Identity;

var hasher = new PasswordHasher<object>();
var hash = hasher.HashPassword(null!, "Admin@123");
Console.WriteLine(hash);
