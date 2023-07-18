import Swal from 'sweetalert2'

function messageBox() {
  function showErrorMessage(message: string) {
    Swal.fire({
      text: message,
      icon: 'error',
      confirmButtonColor: '#a37d1b',
      confirmButtonText: '確定'
    })
  }
  function showSuccess() {
    Swal.fire({
      icon: 'success',
      text: '成功',
      confirmButtonColor: '#a37d1b',
      confirmButtonText: '確定'
    })
  }
  function showSuccessMessage(message: string) {
    Swal.fire({
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
