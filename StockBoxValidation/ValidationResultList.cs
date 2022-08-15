using System;
using System.Collections.Generic;

namespace StockBox.Validation
{
    public class ValidationResultList : List<ValidationResult>
    {
        public bool HasFailures
        {
            get
            {
                foreach (ValidationResult r in this)
                    if (r.IsFailure) return true;
                return false;
            }
        }

        public bool Success
        {
            get
            {
                return !HasFailures;
            }
        }

        public ValidationResultList() { }

        public ValidationResultList(ValidationResultList source)
        {
            AddRange(source);
        }

        public void Add(bool result, string message, object aux = null)
        {
            Add(new ValidationResult(result, message, aux));
        }

        public ValidationResultList GetFailures()
        {
            var ret = new ValidationResultList();
            foreach (var item in this)
                if (item.IsFailure)
                    ret.Add(item);
            return ret;
        }

        public string GetFailureMessages()
        {
            List<string> ret = new List<string>();
            var failures = GetFailures();
            foreach (var item in failures)
                ret.Add(item.Message);
            return String.Join("\r\n", ret);
        }
    }
}
