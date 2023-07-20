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

  function showSuccessMessage(message?: string, hasConfirm?: boolean, timer?: number) {
    const swalOptions = {
      icon: 'success',
      text: message,
      confirmButtonColor: '#a37d1b',
      confirmButtonText: '確定'
    }

    if (message) Object.assign(swalOptions, { text: message })

    if (hasConfirm == false) Object.assign(swalOptions, { showConfirmButton: false })

    if (timer) Object.assign(swalOptions, { timer: timer })

    return Swal.fire(swalOptions as SweetAlertOptions)
  }
  return {
    showSuccessMessage,
    showErrorMessage
  }
}
const MessageBox = messageBox()

export default MessageBox
