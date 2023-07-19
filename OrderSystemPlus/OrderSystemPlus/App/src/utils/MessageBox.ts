import Swal from 'sweetalert2'

function messageBox() {
  async function showErrorMessage(message: string) {
    return await Swal.fire({
      text: message,
      icon: 'error',
      confirmButtonColor: '#a37d1b',
      confirmButtonText: '確定'
    })
  }
  function showSuccess() {
    return Swal.fire({
      icon: 'success',
      text: '成功',
      confirmButtonColor: '#a37d1b',
      confirmButtonText: '確定'
    })
  }
  function showSuccessMessage(message: string) {
    return Swal.fire({
      icon: 'success',
      text: message,
      confirmButtonColor: '#a37d1b',
      confirmButtonText: '確定'
    })
  }
  return {
    showSuccess,
    showSuccessMessage,
    showErrorMessage
  }
}
const MessageBox = messageBox()

export default MessageBox
