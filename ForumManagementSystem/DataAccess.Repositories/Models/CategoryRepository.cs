using AspNetCoreDemo.Models;
using DataAccess.Repositories.Data;
using ForumManagementSystem.Exceptions;
using ForumManagementSystem.Models;

namespace ForumManagementSystem.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext context;
        private readonly IPostRepository postRepository;
        
        public CategoryRepository(ApplicationContext context, IPostRepository postRepository)
        {
            this.context = context;
            this.postRepository = postRepository;
        }
        public Category Create(Category category)
        {
            category.DateTime = DateTime.Now;
            context.Categories.Add(category);
            context.SaveChanges();

            return category;
        }

        public Category Delete(int id)
        {
            var category = GetById(id);
            context.Categories.Remove(category);
            context.SaveChanges();

            return category;
        }

        public PaginatedList<Category> FilterBy(Models.CategoryQueryParameter parameters)
        {
            List<Category> result = context.Categories.ToList();

            if (!string.IsNullOrEmpty(parameters.Name))
            {
                result = result.FindAll(c => c.Name.Contains(parameters.Name));
            }
            if (!string.IsNullOrEmpty(parameters.Description))
            {
                result = result.FindAll(c => c.Name.Contains(parameters.Description));
            }
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                if (parameters.SortBy.Equals("name", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(c => c.Name).ToList();
                }
                else if (parameters.SortBy.Equals("description", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = result.OrderBy(c => c.Description).ToList();
                }
                if (!string.IsNullOrEmpty(parameters.SortOrder) 
                    && parameters.SortOrder.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Reverse();
                }
            }
            if (result.Count==0)
            {
                throw new EntityNotFoundException("These are no category with this name or description.");
            }

            int totalPages = (result.Count() + parameters.PageSize - 1) / parameters.PageSize;

            result = Paginate(result, parameters.PageNumber, parameters.PageSize);

            return new PaginatedList<Category>(result.ToList(), totalPages, parameters.PageNumber);
            }

        public static List<Category> Paginate(List<Category> result, int pageNumber, int pageSize)
        {
            return result
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            return category ?? throw new EntityNotFoundException($"Category with id={id} doesn't exist.");
        }
        public Category GetByName(string name)
        {
            var category = context.Categories.Where(c => c.Name == name).FirstOrDefault();
            return category ?? throw new EntityNotFoundException($"Category with name {name} doesn't exist.");
        }

        public Category Update(int id, Category category)
        {
            Category categoryToUpdate = GetById(id);
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.DateTime = DateTime.Now;
            
            context.SaveChanges();

            return categoryToUpdate;
        }

        public int IncreaseCategoryPostCount(Post post)
        {
            Category category = GetById(post.CategoryId);
            category.CountPosts++;
            context.SaveChanges();
            return category.CountPosts;
        }
        public int DecreaseCategoryPostCount(Post post)
        {
            Category category = GetById(post.CategoryId);
            category.CountPosts--;
            context.SaveChanges();
            return category.CountPosts;
        }

        public int IncreaseCategoryCommentCount(Comment comment)
        {
            Post post = this.postRepository.GetById(comment.PostId);
            Category category = this.GetById(post.CategoryId);
            category.CountComments++;
            context.SaveChanges();
            return category.CountComments;
        }

        public int DecreaseCategoryCommentCount(Comment comment)
        {
            Post post = this.postRepository.GetById(comment.PostId);
            Category category = this.GetById(post.CategoryId);
            category.CountComments--;
            context.SaveChanges();
            return category.CountComments;
        }
    }
}
