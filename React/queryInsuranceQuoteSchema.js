import * as Yup from "yup";

const queryInsuranceQuoteSchema = Yup.object().shape({
  query: Yup.string().min(2),
  startDate: Yup.date().min(1),
  endDate: Yup.date().min(1),
});

export default queryInsuranceQuoteSchema;
