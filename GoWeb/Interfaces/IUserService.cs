using AutoMapper;
using GoWeb.Shared.Models;
using GoWeb.Repositories;
using GoWeb.Сonstants;
using Microsoft.Extensions.Caching.Memory;

namespace GoWeb.Interfaces
{
    public interface IUserService
    {

        public  Task<List<string?>> GetIdUsersDB(int idEvent);
        public  Task<List<UserPrewievDTO>> GetPreviewUsers(List<string>? idUsers);
        public Task<UserPrewievDTO> GetPreviewUser(string idUser);
        public Task<List<UserPrewievDTO>> GetPreviewUsersDB(List<string>? idUsers);
        public void WriteUsersInCache(List<UserPrewievDTO> usersPreview);


     
    }
}
