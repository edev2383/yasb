﻿using System;
using System.Collections.Generic;


namespace StockBox.Validation
{

    /// <summary>
    /// Class <c>ValidationResultList</c> aggregates ValidationResults.
    /// </summary>
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

        public static ValidationResultList CreateSingle(bool result, string message, object aux = null)
        {
            return new ValidationResultList() {
                new ValidationResult(result, message, aux),
            };
        }

        public void Add(bool result, string message, object aux = null)
        {
            Add(new ValidationResult(result, message, aux));
        }

        public void AddFromBool(bool result, string message, object aux = null)
        {
            Add(new ValidationResult(result, message, aux));
        }

        public ValidationResultList GetHasValidationObjectsOfType<T>()
        {
            var ret = new ValidationResultList();
            foreach (var item in this)
                if (item.ValidationObject != null && item.ValidationObject is T)
                    ret.Add(item);
            return ret;
        }

        public List<T> GetValidationObjects<T>()
        {
            var ret = new List<T>();
            foreach (var result in GetHasValidationObjectsOfType<T>())
                ret.Add((T)result.ValidationObject);
            return ret;
        }

        public ValidationResultList GetHasValidationObjects()
        {
            var ret = new ValidationResultList();
            foreach (var item in this)
                if (item.ValidationObject != null)
                    ret.Add(item);
            return ret;
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

        public void Add(IValidationResultProvider result)
        {
            base.Add(new ValidationResult(result.Result, result.Message, result.ValidationObject));
        }
    }
}
