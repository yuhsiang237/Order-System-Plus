import Swal from 'sweetalert2'

function messageBox() {
  function showErrorMessage(message: string) {
    return Swal.fire({
      text: message,
      icon: 'error',
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
    showSuccessMessage,
    showErrorMessage
  }
}
const MessageBox = messageBox()

export default MessageBox
