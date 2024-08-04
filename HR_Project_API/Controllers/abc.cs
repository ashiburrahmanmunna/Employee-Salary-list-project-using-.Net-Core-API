//using Microsoft.AspNetCore.Mvc;

//[HttpGet("EmployeeReport")]
//public async Task<ActionResult> EmployeeReport(string com, string dept, string desig)
//{
//    try
//    {
//        //var companylist = empRepo.Where(x => x.CompanyId == Request.Cookies[ApplicationConst.CompanyHeaderName].ToString());
//        var companylist = empRepo.Where(x => x.CompanyId == com);
//        if (!string.IsNullOrEmpty(dept))
//        {
//            companylist = companylist.Where(x => x.DeptId == dept);
//        }
//        if (!string.IsNullOrEmpty(desig))
//        {
//            companylist = companylist.Where(x => x.DesigId == desig);
//        }
//        var emp = await companylist.Select(x => new EmpreportViewModel
//        {
//            EmpName = x.EmpName,
//            ComName = x.Company.ComName,
//            DeptName = x.Dept.DeptName,
//            Gross = x.Gross,
//            Basic = x.Basic,
//            HRent = x.HRent,
//            Medical = x.Medical,
//            Others = x.Others,
//            dtJoin = x.dtJoin.ToShortDateString(),
//            Gender = x.Gender,
//            ShiftName = x.Shift.ShiftName,
//            jd = x.dtJoin

//        }).ToListAsync();
//        return Ok(emp);
//    }
//    catch (Exception e)
//    {

//        return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
//    }

//}