import * as Yup from "yup";

const addInsuranceQuoteSchema = Yup.object().shape({
  insuranceId: Yup.string().min(1).required("Is Required"),
  citizenship: Yup.string().min(1),
  age: Yup.string().min(1).required("Is Required"),
  mailingAddress: Yup.string().min(1),
  travelDestination: Yup.string().min(1),
  policyRangeId: Yup.string().min(1),
  isArrivedInUSA: Yup.bool(),
  visaTypeId: Yup.string().min(1),
});

export default addInsuranceQuoteSchema;
