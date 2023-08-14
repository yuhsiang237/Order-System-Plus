<template>
  <div>
    <form asp-action="SearchProductCategory" id="form_search" method="get">
      <div class="search-area">
        <div class="container">
          <div class="row">
            <div class="col">
              <div>
                <h1 class="pt-5 pb-3 main-color01">商品分類列表</h1>
              </div>
              <div class="row">
                <div class="col-sm col-md-2 col-lg-2 col-xl-2">
                  <label class="custom-label">分類名稱</label>
                  <div class="form-group form-row">
                    <div class="col">
                      <input
                        id="input_name"
                        type="text"
                        class="form-control mr-2"
                        name="searchStringName"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div class="text-right">
                <button type="submit" class="btn btn-main-color01 mb-2 mr-1">查詢資料</button>
                <button id="btn-clearSearch" class="btn btn-main-color02 outline-btn mb-2">
                  清空查詢
                </button>
              </div>
              <hr class="mt-0" />
              <div class="d-flex justify-content-end">
                <div class="col-12 col-sm-2 px-0">
                  <div class="form-group">
                    <select class="form-control" name="sortOrder">
                      <option value="0">預設排序</option>
                      <option value="1">商品名稱 高→低</option>
                      <option value="2">商品名稱 低→高</option>
                      <option value="3">商品庫存量 高→低</option>
                      <option value="4">商品庫存量 低→高</option>
                      <option value="5">商品價格 高→高</option>
                      <option value="6">商品價格 低→高</option>
                    </select>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="container">
        <div class="row my-3">
          <div class="col">
            <div class="text-right">
              <button id="btn_productCategoryModal" type="button" class="btn btn-main-color01">
                新增分類
              </button>
            </div>
          </div>
        </div>
        <div class="row">
          <div class="col">
            <div class="table-responsive" id="productTable">
              <table class="table table-hover">
                <thead>
                  <tr>
                    <th>分類名稱</th>
                    <th>描述</th>
                    <th>操作</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td>111</td>
                    <td>111</td>
                    <td>
                      <button type="button" class="mr-1 btn btn-main-color02 outline-btn">
                        編輯
                      </button>
                      <button type="button" class="btn btn-red">刪除</button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>

      <div class="list-pagination mt-3">
        <div class="form-inline text-center">
          <div class="mx-auto">
            每頁
            <select id="select_changePageSize" class="custom-select" name="pageSize">
              <option value="10">10</option>
              <option value="30">30</option>
              <option value="50">50</option></select
            >，第 <span>1</span> 頁，共 <span></span> 頁，
            <a class="btn btn-outline-secondary btn-sm @prevDisabled"> 上一頁 </a>｜跳至第
            <select id="select_goToPage" class="custom-select" name="goToPageNumber">
              <option>選擇</option>
              <option value="@i">@i</option>
            </select>
            頁｜
            <a class="btn btn-outline-secondary btn-sm @nextDisabled"> 下一頁 </a>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'

export default defineComponent({
  name: 'productTypeSearch',
  components: {},
  setup() {}
})
</script>
