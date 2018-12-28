using System.Threading.Tasks;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Identity;

namespace CourseApp.API.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IUserRepository _userRepository;
        private IPhotoRepository _photoRepository;
        private IMessageRepository _messageRepository;
        private IAnswerRepository _answerRepository;
        private IUserAnswerRepository _userAnswerRepository;
        private IQuestionRepository _questionRepository;
        private IExamRepository _examRepository;
        private IUserExamRepository _userExamRepository;

        private readonly DataContext _context;

        public RepositoryWrapper(DataContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        public IAnswerRepository AnswerRepository
        {
            get
            {
                if (_answerRepository == null)
                {
                    _answerRepository = new AnswerRepository(_context);
                }
                return _answerRepository;
            }
        }
        public IQuestionRepository QuestionRepository
        {
            get
            {
                if (_questionRepository == null)
                {
                    _questionRepository = new QuestionRepository(_context);
                }
                return _questionRepository;
            }
        }

        public IUserAnswerRepository UserAnswerRepository
        {
            get
            {
                if (_userAnswerRepository == null)
                {
                    _userAnswerRepository = new UserAnswerRepository(_context);
                }
                return _userAnswerRepository;
            }
        }

        public IExamRepository ExamRepository
        {
            get
            {
                if (_examRepository == null)
                {
                    _examRepository = new ExamRepository(_context);
                }
                return _examRepository;
            }
        }





        public IPhotoRepository PhotoRepository
        {
            get
            {
                if (_photoRepository == null)
                {
                    _photoRepository = new PhotoRepository(_context);
                }
                return _photoRepository;
            }
        }
        public IMessageRepository MessageRepository
        {
            get
            {
                if (_messageRepository == null)
                {
                    _messageRepository = new MessageRepository(_context);
                }
                return _messageRepository;
            }
        }
        public IUserExamRepository UserExamRepository
        {
            get
            {
                if (_userExamRepository == null)
                {
                    _userExamRepository = new UserExamRepository(_context);
                }

                return _userExamRepository;
            }
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}