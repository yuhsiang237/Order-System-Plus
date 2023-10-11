function formatToYYYYMMDD(inputDate: Date): string {
  const year = inputDate.getFullYear()
  const month = (inputDate.getMonth() + 1).toString().padStart(2, '0') // +1 是因为月份是从 0 开始的
  const day = inputDate.getDate().toString().padStart(2, '0')

  return `${year}-${month}-${day}`
}

function toDateString(dateString: string): string {
  const originalDate = new Date(dateString)
  const formattedDate = formatToYYYYMMDD(originalDate)
  return formattedDate
}
export default {
  toDateString
}
