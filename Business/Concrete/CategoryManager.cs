using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public IResult Add(Category category)
        {
            throw new NotImplementedException();
        }

        public IResult Delete(Category category)
        {
            throw new NotImplementedException();
        }

        public IDataResult<Category> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<Category>> GetList()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetAll().ToList());
        }

        public IResult Update(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
